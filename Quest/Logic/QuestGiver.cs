using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;//�Ի�����ű�
    QuestData_SO currentQuest;//��ǰ����
    public DialogueData_SO startDialogue;//һ��ʼ�ĶԻ�����
    public DialogueData_SO progressDialogue;//��������еĶԻ�����
    public DialogueData_SO completeDialogue;//�������ʱ�ĶԻ�����
    public DialogueData_SO finshDialogue;//���������ĶԻ�����(��ȡ�꽱����)
    #region ��ȡ����״̬
    //��ȡ����ʼ��״̬
    public bool IsStarted 
    {
        get 
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�������б����Ƿ����������
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;//�оͷ�������ʼ��״̬
            else return false;//û�з���false
        }
    }
    //��ȡ�������ʱ��״̬
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�������б����Ƿ����������
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;//�оͷ���������ɵ�״̬
            else return false;//û�з���false
        }
    }
    //��ȡ���������״̬
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�������б����Ƿ����������
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;//�оͷ������������״̬
            else return false;//û�з���false
        }
    }
    #endregion
    void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    void Start()
    {
        controller.currentData = startDialogue;//��ֵһ��ʼ�ĶԻ�����
        currentQuest = controller.currentData.GetQuest();//�õ���ǰ����
    }
    void Update()
    {
        if (IsStarted) //�������ʼ��
        {
            if (IsComplete) //��ȥ�Ի�ʱ��������Ѿ����
                controller.currentData = completeDialogue;//�������ʱ�ĶԻ�����
            else //��ȥ�Ի�ʱ��û�����
                controller.currentData = progressDialogue;//������������еĶԻ�����
        }
        if (IsFinished) //�����������ˣ���ȡ�꽱���ˣ�
        {
            controller.currentData = finshDialogue;//���ɽ����ĶԻ�����
        }
    }
}
