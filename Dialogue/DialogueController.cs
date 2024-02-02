using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;//�Ի�������
    bool canTalk=false;//�Ƿ���ԶԻ�
    //���봥����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null) 
        {
            canTalk = true;//������ҶԻ����ݲ�Ϊ�տ��ԶԻ�
        }
    }
    //�뿪������
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {//�뿪npc�Ĵ�����Χ�͹رնԻ�UIh
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }
    void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E)) //�ڴ�������Χ����e���ԶԻ�
        {
            OpenDialogue();
        }
    }
    //��ʼ�Ի�
    void OpenDialogue() 
    {
        //����Ի�������Ϣ
        DialogueUI.Instance.UpdateDialogueData(currentData);
        //��UI���
        DialogueUI.Instance.UpdataMainDialogue(currentData.dialoguePiece[0]);
    }
}
