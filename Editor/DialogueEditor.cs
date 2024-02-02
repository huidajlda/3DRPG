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
//EditorWindow需要引用命名空间using UnityEditor;
//创建对话编辑器的自定义窗口，这个窗口和unity的窗口一样，可以拖动，可以作为选项卡
public class DialogueEditor : EditorWindow
{
    DialogueData_SO currentData;//当前的对话数据
    ReorderableList piecesList = null;//用来保存每一条语句块的信息
    Vector2 scrollPos=Vector2.zero;//滑动条的位置
    //对话和选项关联的字典
    Dictionary<string,ReorderableList> optionListDict=new Dictionary<string, ReorderableList>();
    //注意要打开窗口一定要静态函数
    [MenuItem("M-Studio/Dialogue Editor")]//unity菜单中进入窗口的按钮
    //初始化窗口
    public static void Init() 
    {
        DialogueEditor editorWindow=GetWindow<DialogueEditor>("Dialogue Editor");//生成一个窗口,参数时窗口的名字
        editorWindow.autoRepaintOnSceneChange = true;//选择的物体或场景变化时可以重新绘制窗口
    }
    //在Inspector窗口中点按按钮进入窗口的窗口初始化方法
    public static void InitWindow(DialogueData_SO data) 
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");//生成一个窗口,参数时窗口的名字
        editorWindow.currentData = data;//对话数据等于传进来的data
    }
    //双击DialogueData_SO的对象，也可以打开窗口的方法,需要using UnityEditor.Callbacks;
    [OnOpenAsset]
    //instancID:每个文件都有的ID
    public static bool OpenAsset(int instancID,int line) //除了函数名其他都是固定写法
    {
        DialogueData_SO data=EditorUtility.InstanceIDToObject(instancID) as DialogueData_SO;//通过文件ID生成对象
        if (data != null) //说明双击的是DialogueData_SO的文件
        {
            DialogueEditor.InitWindow(data);//打开窗口
            return true;
        }
        return false;
    }
    //当前选着对象更改时会调用的周期函数
    private void OnSelectionChange()
    {
        var newData=Selection.activeObject as DialogueData_SO;
        if (newData != null) //为空则说明选着的数据并不是DialogueData_SO类型的数据
        {
            //重新绘制列表
            currentData = newData;
            SetupReorderableList();
        }
        else 
        {//清空
            currentData = null;
            piecesList = null;
        }
        Repaint();//可以强制调用一次OnGUI函数
    }
    //绘制窗口内容
    void OnGUI() 
    {
        if (currentData != null)
        {
            //TextField：文字在窗口可以修改 LabelField:文字在窗口不可以修改
            //EditorGUILayout.TextField(currentData.name, EditorStyles.boldLabel);//粗体显示文件名的文字
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);//在窗口中绘制文件名字，粗体显示
            GUILayout.Space(10);//空出多大的距离
            //开始画滑动条
            scrollPos=GUILayout.BeginScrollView(scrollPos,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            if (piecesList == null)//保存对话列表的变量是否为空
                SetupReorderableList();
            piecesList.DoLayoutList();//画出对话内容
            GUILayout.EndScrollView();//结束画滑动条
        }
        else 
        {
            if (GUILayout.Button("Create New Dialogue")) //绘制一个创建文件的按钮
            {
                //按下按钮后执行的代码
                string dataPath = "Assets/Game Data/Dialogue Data/";//文件路径
                if (!Directory.Exists(dataPath)) //是否存在这个文件夹
                    Directory.CreateDirectory(dataPath);//不存在则创建这个文件夹
                DialogueData_SO newData=ScriptableObject.CreateInstance<DialogueData_SO>();//创建数据
                AssetDatabase.CreateAsset(newData,dataPath+"/"+"New Dialogue.asset");//创建文件
                currentData = newData;
            }
            GUILayout.Label("没有选中对象",EditorStyles.boldLabel);//粗体显示文字(不可以修改)
        }
    }
    //关闭窗口OnDisable在GUI中表示关闭窗口时执行
    void OnDisable()
    {
        optionListDict.Clear();//清空字典
    }
    //绘制内容的方法
    private void SetupReorderableList() 
    {
        //四个true分别表示：是否可以拖拽，是否可以显示头命名，是否有添加的按钮，是否可以被删除
        piecesList = new ReorderableList(currentData.dialoguePiece, typeof(DialoguePiece), true, true, true, true);
        piecesList.drawHeaderCallback += OnDrawPieceHeader;//绘制标题
        piecesList.drawElementCallback += OnDrawPieceListElement;//绘制对话内容
        piecesList.elementHeightCallback += OnHeightChanged;//调整高度
    }
    //根据对话内容来改变高度
    private float OnHeightChanged(int index)
    {
        return GetPieceHeight(currentData.dialoguePiece[index]);
    }
    //计算高度(根据对话内容转换高度)
    float GetPieceHeight(DialoguePiece piece) 
    {
        var height=EditorGUIUtility.singleLineHeight;
        var isExpand = piece.canExpand;
        if (isExpand) //为真时（即没有折叠起来）才调整高度
        {
            height += EditorGUIUtility.singleLineHeight * 9;//默认的对话的高度画9行
            var options = piece.dialogueOptions;
            if (options.Count > 1) //选项的高度
            {
                height += EditorGUIUtility.singleLineHeight * options.Count;
            }
        }
        return height;
    }
    //绘制对话内容的方法(身体)
    private void OnDrawPieceListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        //标记(标记之后就可以保存，撤销等等正常文件的操作)
        EditorUtility.SetDirty(currentData);
        //给创建一个样式，括号里的TextField表示此样式是给创建的TextField使用的
        GUIStyle textStyle = new GUIStyle("TextField");
        //Rect rect包含了在窗口的位置和大小
        if (index < currentData.dialoguePiece.Count) //index就是选中的对话文件中的对话的数量，防止多画
        {
            //只考虑画单个栏目的内容，其他栏目会自动生成
            var currentPiece = currentData.dialoguePiece[index];
            var tempRect = rect;
            //画对话ID部分
            tempRect.height=EditorGUIUtility.singleLineHeight;//默认的单行高度
            //画出折叠按钮 currentPiece.ID是折叠按钮显示的名字
            currentPiece.canExpand = EditorGUI.Foldout(tempRect, currentPiece.canExpand, currentPiece.ID);
            if (currentPiece.canExpand) //当折叠为真时，才显示具体的内容，不然就显示一个折叠内容
            {
                tempRect.width = 30;//调整宽度
                tempRect.y += tempRect.height;//调整高度，避免和堆叠按钮重合
                EditorGUI.LabelField(tempRect, "ID");//画上对话的ID文字
                tempRect.x += tempRect.width;//将临时变量的位置移动30的距离
                tempRect.width = 100;//一个更宽的框
                currentPiece.ID = EditorGUI.TextField(tempRect, currentPiece.ID);//画上ID对应的内容
                //画任务部分
                tempRect.x += tempRect.width + 10;
                EditorGUI.LabelField(tempRect, "Quest");
                tempRect.x += tempRect.width;
                //ObjectField让其可以选着文件，最后一个bool值是：是否可以选着场景中的物体
                currentPiece.quest = (QuestData_SO)EditorGUI.ObjectField(tempRect, currentPiece.quest, typeof(QuestData_SO), false);
                //画对话头像部分
                tempRect.y += EditorGUIUtility.singleLineHeight + 5;//换行画，且和上面抱持5的间距
                tempRect.x = rect.x;//回到窗口左边的会话位置
                tempRect.height = 60;
                tempRect.width = tempRect.height;
                currentPiece.image = (Sprite)EditorGUI.ObjectField(tempRect, currentPiece.image, typeof(Sprite), false);
                //画文本框部分
                tempRect.x += tempRect.width + 5;
                tempRect.width = rect.width - tempRect.x;
                textStyle.wordWrap = true;//让文本框的文字可以自动回行的样式
                //给文本框添加自动换行样式
                currentPiece.text = (string)EditorGUI.TextField(tempRect, currentPiece.text, textStyle);
                //画选项（在列表中画另一个列表）
                tempRect.y += tempRect.height + 5;
                tempRect.x = rect.x;
                tempRect.width = rect.width;
                string optionListKey = currentPiece.ID + currentPiece.text;//字典的键值
                if (optionListKey != string.Empty) //如果不为空
                {
                    if (!optionListDict.ContainsKey(optionListKey)) //不包含在字典中
                    {
                        var optionList = new ReorderableList(currentPiece.dialogueOptions, typeof(DialogueOption), true, true, true, true);
                        optionList.drawHeaderCallback = OnDrawOptionHeader;
                        //回调函数的参数会匹配对应的类型
                        //画选项的每个元素的回调
                        optionList.drawElementCallback = (optionRect,optionIndex,optionActive,optionFocused) =>
                        {
                            OnDrawOptionElement(currentPiece, optionRect, optionIndex, optionActive, optionFocused);
                        };
                        optionListDict[optionListKey] = optionList;//在字典中保存信息
                    }
                    optionListDict[optionListKey].DoList(tempRect);//可以包含在其他元素里面画

                }
            }
        }
    }
    //画选项列表的头部
    private void OnDrawOptionHeader(Rect rect)
    {
        GUI.Label(rect, "Option Text");
        rect.x += rect.width*0.5f+10;
        GUI.Label(rect, "Target ID");
        rect.x += rect.width * 0.3f;
        GUI.Label(rect, "Apply");
    }

    //画选项元素的方法
    private void OnDrawOptionElement(DialoguePiece currentPiece, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var currentOption = currentPiece.dialogueOptions[optionIndex];
        var tempRect = optionRect;
        //画选项对话的文本框
        tempRect.width = optionRect.width * 0.5f;//占50%的宽度
        currentOption.text = EditorGUI.TextField(tempRect, currentOption.text);
        //画下一跳对话的ID
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect, currentOption.targetID);
        //画是否时任务对话的按钮
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeQuest = EditorGUI.Toggle(tempRect, currentOption.takeQuest);
    }

    //绘制头部的回调方法
    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect, "标题栏--对话内容");
    }
}
//可以新键一个脚本来写，这里图省事了
[CustomEditor(typeof(DialogueData_SO))]//一定要写的，这个Editor影响的是哪个数据
public class DialogueCustomEditor : Editor //这个是为了改变在检测窗口里显示的样子
{
    public override void OnInspectorGUI()//在检测器(Inspector)窗口绘制的样子
    {
        if (GUILayout.Button("Open in Editor")) //创建一个按钮名字为Open in Editor，在if里可以判断其有没有被点按
        {
            //target是Editor中经常使用的一个变量参数，表示当前选中的物体的意思
            DialogueEditor.InitWindow((DialogueData_SO)target);
        }
        base.OnInspectorGUI();//保留基础绘制(即脚本中的数据变量之类的)
    }
}
