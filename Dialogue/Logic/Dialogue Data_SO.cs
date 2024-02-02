using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New DialogueData",menuName ="Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePiece=new List<DialoguePiece>();//�Ի���Ϣ���б�
    //�Ի���Ϣ���ֵ�
    public Dictionary<string,DialoguePiece> dialogueIndex=new Dictionary<string,DialoguePiece>();
#if UNITY_EDITOR//ȷ����unity��ִ�в�Ӱ����
    //���������unity���õģ���ǰ��������unity�����б������˲Ż�ִ��
    void OnValidate() 
    {
        dialogueIndex.Clear();//����ֵ�
        foreach (var piece in dialoguePiece) 
        {
            if (!dialogueIndex.ContainsKey(piece.ID)) 
            {
                dialogueIndex.Add(piece.ID, piece);//��ӽ��ֵ�
            }
        }
    }
#endif
    //��ȡ�Ի��е�����
    public QuestData_SO GetQuest() 
    {
        QuestData_SO currentQuest = null;
        foreach (var piece in dialoguePiece) //����ÿ��Ի�
        {
            if(piece.quest!=null)//�������Ի�������
                currentQuest = piece.quest;
        }
        return currentQuest;//�����񷵻���������û�з��ؿ�
    }
}
