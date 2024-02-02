using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialoguePiece
{
    public string ID;//�����Ի���ID���
    public Sprite image;//ͼƬ(ͷ�����)
    [TextArea]
    public string text;//�ı�
    public QuestData_SO quest;//�öԻ�����������
    [HideInInspector]//�ڼ����������
    public bool canExpand;//���Զѵ������Լ����õ�һ������õģ�
    //�Ի�ѡ����б�
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();
}
