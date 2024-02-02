using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngineInternal;
using Unity.VisualScripting.ReorderableList.Internal;
using UnityEditorInternal;
using System.IO;
using System;
using Unity.VisualScripting;
//EditorWindow��Ҫ���������ռ�using UnityEditor;
//�����Ի��༭�����Զ��崰�ڣ�������ں�unity�Ĵ���һ���������϶���������Ϊѡ�
public class DialogueEditor : EditorWindow
{
    DialogueData_SO currentData;//��ǰ�ĶԻ�����
    ReorderableList piecesList = null;//��������ÿһ���������Ϣ
    Vector2 scrollPos=Vector2.zero;//��������λ��
    //�Ի���ѡ��������ֵ�
    Dictionary<string,ReorderableList> optionListDict=new Dictionary<string, ReorderableList>();
    //ע��Ҫ�򿪴���һ��Ҫ��̬����
    [MenuItem("M-Studio/Dialogue Editor")]//unity�˵��н��봰�ڵİ�ť
    //��ʼ������
    public static void Init() 
    {
        DialogueEditor editorWindow=GetWindow<DialogueEditor>("Dialogue Editor");//����һ������,����ʱ���ڵ�����
        editorWindow.autoRepaintOnSceneChange = true;//ѡ�������򳡾��仯ʱ�������»��ƴ���
    }
    //��Inspector�����е㰴��ť���봰�ڵĴ��ڳ�ʼ������
    public static void InitWindow(DialogueData_SO data) 
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");//����һ������,����ʱ���ڵ�����
        editorWindow.currentData = data;//�Ի����ݵ��ڴ�������data
    }
    //˫��DialogueData_SO�Ķ���Ҳ���Դ򿪴��ڵķ���,��Ҫusing UnityEditor.Callbacks;
    [OnOpenAsset]
    //instancID:ÿ���ļ����е�ID
    public static bool OpenAsset(int instancID,int line) //���˺������������ǹ̶�д��
    {
        DialogueData_SO data=EditorUtility.InstanceIDToObject(instancID) as DialogueData_SO;//ͨ���ļ�ID���ɶ���
        if (data != null) //˵��˫������DialogueData_SO���ļ�
        {
            DialogueEditor.InitWindow(data);//�򿪴���
            return true;
        }
        return false;
    }
    //��ǰѡ�Ŷ������ʱ����õ����ں���
    private void OnSelectionChange()
    {
        var newData=Selection.activeObject as DialogueData_SO;
        if (newData != null) //Ϊ����˵��ѡ�ŵ����ݲ�����DialogueData_SO���͵�����
        {
            //���»����б�
            currentData = newData;
            SetupReorderableList();
        }
        else 
        {//���
            currentData = null;
            piecesList = null;
        }
        Repaint();//����ǿ�Ƶ���һ��OnGUI����
    }
    //���ƴ�������
    void OnGUI() 
    {
        if (currentData != null)
        {
            //TextField�������ڴ��ڿ����޸� LabelField:�����ڴ��ڲ������޸�
            //EditorGUILayout.TextField(currentData.name, EditorStyles.boldLabel);//������ʾ�ļ���������
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);//�ڴ����л����ļ����֣�������ʾ
            GUILayout.Space(10);//�ճ����ľ���
            //��ʼ��������
            scrollPos=GUILayout.BeginScrollView(scrollPos,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            if (piecesList == null)//����Ի��б�ı����Ƿ�Ϊ��
                SetupReorderableList();
            piecesList.DoLayoutList();//�����Ի�����
            GUILayout.EndScrollView();//������������
        }
        else 
        {
            if (GUILayout.Button("Create New Dialogue")) //����һ�������ļ��İ�ť
            {
                //���°�ť��ִ�еĴ���
                string dataPath = "Assets/Game Data/Dialogue Data/";//�ļ�·��
                if (!Directory.Exists(dataPath)) //�Ƿ��������ļ���
                    Directory.CreateDirectory(dataPath);//�������򴴽�����ļ���
                DialogueData_SO newData=ScriptableObject.CreateInstance<DialogueData_SO>();//��������
                AssetDatabase.CreateAsset(newData,dataPath+"/"+"New Dialogue.asset");//�����ļ�
                currentData = newData;
            }
            GUILayout.Label("û��ѡ�ж���",EditorStyles.boldLabel);//������ʾ����(�������޸�)
        }
    }
    //�رմ���OnDisable��GUI�б�ʾ�رմ���ʱִ��
    void OnDisable()
    {
        optionListDict.Clear();//����ֵ�
    }
    //�������ݵķ���
    private void SetupReorderableList() 
    {
        //�ĸ�true�ֱ��ʾ���Ƿ������ק���Ƿ������ʾͷ�������Ƿ�����ӵİ�ť���Ƿ���Ա�ɾ��
        piecesList = new ReorderableList(currentData.dialoguePiece, typeof(DialoguePiece), true, true, true, true);
        piecesList.drawHeaderCallback += OnDrawPieceHeader;//���Ʊ���
        piecesList.drawElementCallback += OnDrawPieceListElement;//���ƶԻ�����
        piecesList.elementHeightCallback += OnHeightChanged;//�����߶�
    }
    //���ݶԻ��������ı�߶�
    private float OnHeightChanged(int index)
    {
        return GetPieceHeight(currentData.dialoguePiece[index]);
    }
    //����߶�(���ݶԻ�����ת���߶�)
    float GetPieceHeight(DialoguePiece piece) 
    {
        var height=EditorGUIUtility.singleLineHeight;
        var isExpand = piece.canExpand;
        if (isExpand) //Ϊ��ʱ����û���۵��������ŵ����߶�
        {
            height += EditorGUIUtility.singleLineHeight * 9;//Ĭ�ϵĶԻ��ĸ߶Ȼ�9��
            var options = piece.dialogueOptions;
            if (options.Count > 1) //ѡ��ĸ߶�
            {
                height += EditorGUIUtility.singleLineHeight * options.Count;
            }
        }
        return height;
    }
    //���ƶԻ����ݵķ���(����)
    private void OnDrawPieceListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        //���(���֮��Ϳ��Ա��棬�����ȵ������ļ��Ĳ���)
        EditorUtility.SetDirty(currentData);
        //������һ����ʽ���������TextField��ʾ����ʽ�Ǹ�������TextFieldʹ�õ�
        GUIStyle textStyle = new GUIStyle("TextField");
        //Rect rect�������ڴ��ڵ�λ�úʹ�С
        if (index < currentData.dialoguePiece.Count) //index����ѡ�еĶԻ��ļ��еĶԻ�����������ֹ�໭
        {
            //ֻ���ǻ�������Ŀ�����ݣ�������Ŀ���Զ�����
            var currentPiece = currentData.dialoguePiece[index];
            var tempRect = rect;
            //���Ի�ID����
            tempRect.height=EditorGUIUtility.singleLineHeight;//Ĭ�ϵĵ��и߶�
            //�����۵���ť currentPiece.ID���۵���ť��ʾ������
            currentPiece.canExpand = EditorGUI.Foldout(tempRect, currentPiece.canExpand, currentPiece.ID);
            if (currentPiece.canExpand) //���۵�Ϊ��ʱ������ʾ��������ݣ���Ȼ����ʾһ���۵�����
            {
                tempRect.width = 30;//�������
                tempRect.y += tempRect.height;//�����߶ȣ�����Ͷѵ���ť�غ�
                EditorGUI.LabelField(tempRect, "ID");//���϶Ի���ID����
                tempRect.x += tempRect.width;//����ʱ������λ���ƶ�30�ľ���
                tempRect.width = 100;//һ������Ŀ�
                currentPiece.ID = EditorGUI.TextField(tempRect, currentPiece.ID);//����ID��Ӧ������
                //�����񲿷�
                tempRect.x += tempRect.width + 10;
                EditorGUI.LabelField(tempRect, "Quest");
                tempRect.x += tempRect.width;
                //ObjectField�������ѡ���ļ������һ��boolֵ�ǣ��Ƿ����ѡ�ų����е�����
                currentPiece.quest = (QuestData_SO)EditorGUI.ObjectField(tempRect, currentPiece.quest, typeof(QuestData_SO), false);
                //���Ի�ͷ�񲿷�
                tempRect.y += EditorGUIUtility.singleLineHeight + 5;//���л����Һ����汧��5�ļ��
                tempRect.x = rect.x;//�ص�������ߵĻỰλ��
                tempRect.height = 60;
                tempRect.width = tempRect.height;
                currentPiece.image = (Sprite)EditorGUI.ObjectField(tempRect, currentPiece.image, typeof(Sprite), false);
                //���ı��򲿷�
                tempRect.x += tempRect.width + 5;
                tempRect.width = rect.width - tempRect.x;
                textStyle.wordWrap = true;//���ı�������ֿ����Զ����е���ʽ
                //���ı�������Զ�������ʽ
                currentPiece.text = (string)EditorGUI.TextField(tempRect, currentPiece.text, textStyle);
                //��ѡ����б��л���һ���б�
                tempRect.y += tempRect.height + 5;
                tempRect.x = rect.x;
                tempRect.width = rect.width;
                string optionListKey = currentPiece.ID + currentPiece.text;//�ֵ�ļ�ֵ
                if (optionListKey != string.Empty) //�����Ϊ��
                {
                    if (!optionListDict.ContainsKey(optionListKey)) //���������ֵ���
                    {
                        var optionList = new ReorderableList(currentPiece.dialogueOptions, typeof(DialogueOption), true, true, true, true);
                        optionList.drawHeaderCallback = OnDrawOptionHeader;
                        //�ص������Ĳ�����ƥ���Ӧ������
                        //��ѡ���ÿ��Ԫ�صĻص�
                        optionList.drawElementCallback = (optionRect,optionIndex,optionActive,optionFocused) =>
                        {
                            OnDrawOptionElement(currentPiece, optionRect, optionIndex, optionActive, optionFocused);
                        };
                        optionListDict[optionListKey] = optionList;//���ֵ��б�����Ϣ
                    }
                    optionListDict[optionListKey].DoList(tempRect);//���԰���������Ԫ�����滭

                }
            }
        }
    }
    //��ѡ���б��ͷ��
    private void OnDrawOptionHeader(Rect rect)
    {
        GUI.Label(rect, "Option Text");
        rect.x += rect.width*0.5f+10;
        GUI.Label(rect, "Target ID");
        rect.x += rect.width * 0.3f;
        GUI.Label(rect, "Apply");
    }

    //��ѡ��Ԫ�صķ���
    private void OnDrawOptionElement(DialoguePiece currentPiece, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var currentOption = currentPiece.dialogueOptions[optionIndex];
        var tempRect = optionRect;
        //��ѡ��Ի����ı���
        tempRect.width = optionRect.width * 0.5f;//ռ50%�Ŀ��
        currentOption.text = EditorGUI.TextField(tempRect, currentOption.text);
        //����һ���Ի���ID
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect, currentOption.targetID);
        //���Ƿ�ʱ����Ի��İ�ť
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeQuest = EditorGUI.Toggle(tempRect, currentOption.takeQuest);
    }

    //����ͷ���Ļص�����
    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect, "������--�Ի�����");
    }
}
//�����¼�һ���ű���д������ͼʡ����
[CustomEditor(typeof(DialogueData_SO))]//һ��Ҫд�ģ����EditorӰ������ĸ�����
public class DialogueCustomEditor : Editor //�����Ϊ�˸ı��ڼ�ⴰ������ʾ������
{
    public override void OnInspectorGUI()//�ڼ����(Inspector)���ڻ��Ƶ�����
    {
        if (GUILayout.Button("Open in Editor")) //����һ����ť����ΪOpen in Editor����if������ж�����û�б��㰴
        {
            //target��Editor�о���ʹ�õ�һ��������������ʾ��ǰѡ�е��������˼
            DialogueEditor.InitWindow((DialogueData_SO)target);
        }
        base.OnInspectorGUI();//������������(���ű��е����ݱ���֮���)
    }
}
