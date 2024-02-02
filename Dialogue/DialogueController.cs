using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;//对话的数据
    bool canTalk=false;//是否可以对话
    //进入触发器
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null) 
        {
            canTalk = true;//是玩家且对话数据不为空可以对话
        }
    }
    //离开触发器
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {//离开npc的触发范围就关闭对话UIh
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }
    void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E)) //在触发器范围按下e可以对话
        {
            OpenDialogue();
        }
    }
    //开始对话
    void OpenDialogue() 
    {
        //传入对话内容信息
        DialogueUI.Instance.UpdateDialogueData(currentData);
        //打开UI面板
        DialogueUI.Instance.UpdataMainDialogue(currentData.dialoguePiece[0]);
    }
}
