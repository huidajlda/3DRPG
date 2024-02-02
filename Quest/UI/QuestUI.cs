using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemToolTip toolTip;//物品描述文本的脚本（是鼠标移到物品上显示物品描述的脚本）
    bool isOpen;//是否打开
    [Header("任务按钮")]
    public RectTransform questListTransform;//任务列表里面的任务按钮生成的位置
    public QuestNameButton questNameButton;//生成的按钮的预设体
    [Header("文本内容")]
    public Text questContentText;//任务详情
    [Header("任务进度")]
    public RectTransform requireTransform;//任务进度生成的位置
    public QuestRequirement requirment;//任务进度的文本内容脚本
    [Header("奖励详情")]
    public RectTransform rewardTransform;//生成奖励格子的位置
    public ItemUI rewardUI;//奖励格子
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //按下T键打开任务UI
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);//显示或失活
            questContentText.text = "";//任务详情等于空，刚打开不显示
            //显示任务列表面板内容
            SetupQuestList();
            if (!isOpen) //没有打开面板则将提示关闭
            {
                toolTip.gameObject.SetActive(false);
            }
        }
    }
    //显示任务列表面板内容的方法
    public void SetupQuestList() 
    {
        //先遍历任务列表的按钮，将其销毁
        foreach (Transform item in questListTransform) 
        {
            Destroy(item.gameObject);
        }
        //遍历任务奖励格子，将其销毁
        foreach (Transform item in rewardTransform) 
        {
            Destroy(item.gameObject);
        }
        //将任务进度显示也清楚，保存打开面板时不会有原来的东西
        foreach (Transform item in requireTransform) 
        {
            Destroy(item.gameObject);
        }
        //根据任务列表的数据生成任务按钮
        foreach (var task in QuestManager.Instance.tasks) 
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupNameButton(task.questData);//调用按钮设置任务文本的方法
            newTask.questContentText=questContentText;//设置任务详情
        }
    }
    //显示任务进度的方法
    public void SetupRequireList(QuestData_SO questData) 
    {
        //保证任务进度是最新的
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        //生成任务进度
        foreach (var require in questData.questRequires) 
        {
            var q = Instantiate(requirment, requireTransform);
            if (questData.isFinished)//如果任务结束了
                q.SetupRequirement(require.name, questData.isFinished);//调用设置任务进度文本的方法的重载
            else//调用设置任务进度文本的方法
                q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);
        }
    }
    //显示奖励物品
    public void SetupRewardItem(ItemData_SO itemData,int amount) 
    {
        var item = Instantiate(rewardUI, rewardTransform);//生成奖励
        item.SetupItemUI(itemData, amount);//设置奖励图片和数量
    }
}
