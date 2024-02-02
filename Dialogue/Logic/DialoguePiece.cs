using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialoguePiece
{
    public string ID;//这条对话的ID序号
    public Sprite image;//图片(头像或奖励)
    [TextArea]
    public string text;//文本
    public QuestData_SO quest;//该对话包含的任务
    [HideInInspector]//在检查器中隐藏
    public bool canExpand;//可以堆叠（给自己设置的一个插件用的）
    //对话选项的列表
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();
}
