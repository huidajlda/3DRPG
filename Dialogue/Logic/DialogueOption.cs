using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueOption
{
    public string text;//回答的文本
    public string targetID;//该回答跳转到哪个文本(piece)上
    public bool takeQuest;//是否接受任务
}
