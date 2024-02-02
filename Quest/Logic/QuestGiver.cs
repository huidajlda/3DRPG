using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;//对话管理脚本
    QuestData_SO currentQuest;//当前任务
    public DialogueData_SO startDialogue;//一开始的对话数据
    public DialogueData_SO progressDialogue;//任务进行中的对话数据
    public DialogueData_SO completeDialogue;//任务完成时的对话数据
    public DialogueData_SO finshDialogue;//任务结束后的对话数据(领取完奖励后)
    #region 获取任务状态
    //获取任务开始的状态
    public bool IsStarted 
    {
        get 
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//看任务列表中是否有这个任务
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;//有就返回任务开始的状态
            else return false;//没有返回false
        }
    }
    //获取任务完成时的状态
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//看任务列表中是否有这个任务
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;//有就返回任务完成的状态
            else return false;//没有返回false
        }
    }
    //获取任务结束的状态
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//看任务列表中是否有这个任务
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;//有就返回任务结束的状态
            else return false;//没有返回false
        }
    }
    #endregion
    void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    void Start()
    {
        controller.currentData = startDialogue;//赋值一开始的对话数据
        currentQuest = controller.currentData.GetQuest();//拿到当前任务
    }
    void Update()
    {
        if (IsStarted) //如果任务开始了
        {
            if (IsComplete) //回去对话时如果任务已经完成
                controller.currentData = completeDialogue;//换成完成时的对话数据
            else //回去对话时还没有完成
                controller.currentData = progressDialogue;//换成任务进行中的对话数据
        }
        if (IsFinished) //如果任务结束了（领取完奖励了）
        {
            controller.currentData = finshDialogue;//换成结束的对话数据
        }
    }
}
