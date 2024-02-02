using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;//选项的文本
    private Button thisButton;//选项的按钮
    private DialoguePiece currentPiece;//选项匹配的对话
    private string nextPieceID;//选项下一条对话的ID
    private bool takeQuest;//是否接受任务
    void Awake()
    {
        thisButton=GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);//加入点击事件
    }
    //更新选项UI
    public void UpdateOption(DialoguePiece piece,DialogueOption option) 
    {
        currentPiece= piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest= option.takeQuest;//这个对话接不接受任务的状态
    }
    //点击按钮的方法
    public void OnOptionClicked() 
    {
        if (currentPiece.quest != null) //说明这条对话有任务
        {
            //这样是为任务数据修改时不影响原始的数据
            var newTask = new QuestManager.QuestTask();//创建任务列表变量
            newTask.questData=Instantiate(currentPiece.quest);//将当前的任务数据赋值给变量里面的任务数据
            if (takeQuest) //接受任务
            {
                //判断任务列表里是否已经有该任务了
                if (QuestManager.Instance.HaveQuest(newTask.questData)) 
                {
                    //判断是否完成给予奖励
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete) //任务完成
                    {
                        newTask.questData.GiveRewards();//调用给与奖励的方法
                        //将该任务状态设置为结束了
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }

                }
                else //添加到任务列表里
                {
                    QuestManager.Instance.tasks.Add(newTask);//添加进任务列表
                    //注意，如果使用newTask.IsStarted的话只是修改了这局部的任务状态，所以需要写一个查找的方法来修改状态
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;//修改任务的状态
                    //检测背包或快捷栏中有没有相应的物品
                    foreach (var requireItem in newTask.questData.RequireTargetName()) 
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }
        if (nextPieceID == "") //没有下一条
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//关闭对话框
            return;
        }
        else 
        {
            //调用更新对话框的方法，更新到指定的对话
            DialogueUI.Instance.UpdataMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
