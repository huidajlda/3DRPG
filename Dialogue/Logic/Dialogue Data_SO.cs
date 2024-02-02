using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New DialogueData",menuName ="Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePiece=new List<DialoguePiece>();//对话信息的列表
    //对话消息的字典
    public Dictionary<string,DialoguePiece> dialogueIndex=new Dictionary<string,DialoguePiece>();
#if UNITY_EDITOR//确保在unity中执行不影响打包
    //这个函数是unity内置的，当前的数据在unity窗口中被更改了才会执行
    void OnValidate() 
    {
        dialogueIndex.Clear();//清空字典
        foreach (var piece in dialoguePiece) 
        {
            if (!dialogueIndex.ContainsKey(piece.ID)) 
            {
                dialogueIndex.Add(piece.ID, piece);//添加进字典
            }
        }
    }
#endif
    //获取对话中的任务
    public QuestData_SO GetQuest() 
    {
        QuestData_SO currentQuest = null;
        foreach (var piece in dialoguePiece) //遍历每句对话
        {
            if(piece.quest!=null)//如果这个对话有任务
                currentQuest = piece.quest;
        }
        return currentQuest;//有任务返回任务数据没有返回空
    }
}
