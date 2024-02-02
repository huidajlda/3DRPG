using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//按钮的显示的文本
    public QuestData_SO currentData;//当前任务数据
    public Text questContentText;//任务详情
    void Awake()
    {
        //添加点击事件
        GetComponent<Button>().onClick.AddListener(updateQuestContent);
    }
    //设置任务按钮名字
    public void SetupNameButton(QuestData_SO questData) 
    {
        currentData = questData;
        if (questData.isComplete)//任务完成就显示任务的名字+完成
            questNameText.text = questData.questName + "(完成)";
        else
            questNameText.text = questData.questName;//没完成就显示任务名字
    }
    //按下任务按钮,显示任务详细信息
    void updateQuestContent() 
    {
        questContentText.text = currentData.description;//设置任务详情文本
        QuestUI.Instance.SetupRequireList(currentData);//显示任务进度
        foreach (Transform item in QuestUI.Instance.rewardTransform) 
        {//删除上一个任务的奖励
            Destroy(item.gameObject);
        }
        foreach (var item in currentData.reward) //显示奖励列表中的每个奖励
        {
            QuestUI.Instance.SetupRewardItem(item.itemData, item.amount);
        }
    }

}
