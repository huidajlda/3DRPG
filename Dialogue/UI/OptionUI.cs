using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;//ѡ����ı�
    private Button thisButton;//ѡ��İ�ť
    private DialoguePiece currentPiece;//ѡ��ƥ��ĶԻ�
    private string nextPieceID;//ѡ����һ���Ի���ID
    private bool takeQuest;//�Ƿ��������
    void Awake()
    {
        thisButton=GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);//�������¼�
    }
    //����ѡ��UI
    public void UpdateOption(DialoguePiece piece,DialogueOption option) 
    {
        currentPiece= piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest= option.takeQuest;//����Ի��Ӳ����������״̬
    }
    //�����ť�ķ���
    public void OnOptionClicked() 
    {
        if (currentPiece.quest != null) //˵�������Ի�������
        {
            //������Ϊ���������޸�ʱ��Ӱ��ԭʼ������
            var newTask = new QuestManager.QuestTask();//���������б����
            newTask.questData=Instantiate(currentPiece.quest);//����ǰ���������ݸ�ֵ�������������������
            if (takeQuest) //��������
            {
                //�ж������б����Ƿ��Ѿ��и�������
                if (QuestManager.Instance.HaveQuest(newTask.questData)) 
                {
                    //�ж��Ƿ���ɸ��轱��
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete) //�������
                    {
                        newTask.questData.GiveRewards();//���ø��뽱���ķ���
                        //��������״̬����Ϊ������
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }

                }
                else //��ӵ������б���
                {
                    QuestManager.Instance.tasks.Add(newTask);//��ӽ������б�
                    //ע�⣬���ʹ��newTask.IsStarted�Ļ�ֻ���޸�����ֲ�������״̬��������Ҫдһ�����ҵķ������޸�״̬
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;//�޸������״̬
                    //��ⱳ������������û����Ӧ����Ʒ
                    foreach (var requireItem in newTask.questData.RequireTargetName()) 
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }
        if (nextPieceID == "") //û����һ��
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//�رնԻ���
            return;
        }
        else 
        {
            //���ø��¶Ի���ķ��������µ�ָ���ĶԻ�
            DialogueUI.Instance.UpdataMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
