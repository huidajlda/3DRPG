using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    public string questName;//任务的名字
    [TextArea]
    public string description;//任务的描述
    //任务的状态
    public bool isStarted;//任务开始
    public bool isComplete;//任务完成(进行中)
    public bool isFinished;//任务结束(领取完奖励)
    //任务的目标
    [System.Serializable]
    public class QuestRequire 
    {
        public string name;//目标的名字
        public int requireAmount;//目标数量
        public int currentAmount;//已完成的数量
    }
    //任务目标的列表
    public List<QuestRequire> questRequires = new List<QuestRequire>();
    //任务奖励列表
    public List<InventoryItem> reward =new List<InventoryItem>();
    //检测任务进度是否完成的方法
    public void CheckQuestProgress() 
    {
        //where选择条件并且返回,是using System.Linq;里面的方法
        //返回的是一个没有序号的列表，不能用for循环只能用foreach
        //这句话就是返回：任务目标数量小于等于已完成的数量的任务目标(即完成了的任务目标)
        //小于等于是因为可能一个任务目标会在多个任务，数量需求少的会先完成，之后就大于任务目标数量了
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        //完成的任务目标等于任务目标列表的数量，表示当前任务完成了
        isComplete = finishRequires.Count()==questRequires.Count();
        if (isComplete) //如果任务完成
        {
            Debug.Log("wanc0");
        }
    }
    //任务需要 消灭/收集 的目标名字的列表
    public List<string> RequireTargetName() 
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires) 
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
    //完成任务给予奖励的方法
    public void GiveRewards() 
    {
        foreach (var reward in reward) //遍历奖励列表
        {
            if (reward.amount < 0) //奖励数量小于0，说明这是任务要扣除的物品
            {
                int requireCount = Mathf.Abs(reward.amount);//取绝对值
                //背包里面有这个物品
                if (InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)
                {
                    //背包里面的数量小于等于任务需求(说明有一部分在快捷栏里面)
                    if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        //将背包中的数量先扣除
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        //不管是刚好还是不够，扣除之后背包中该物品的数量都应该是0
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;
                        if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                            //快捷栏上有的话，将补上剩余需要的数量
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                    }
                    else//背包里面的数量足够直接减去
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount-=requireCount;
                }
                else //背包里面找不到这个物品
                {
                    //因为是给奖励的方法，在调用时说明任务已经完成,那么背包没有说明物品在快捷栏上面
                    //在快捷栏上减去需要扣除的任务数量
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount-=requireCount;
                }
            }
            else//数量大于0，则是奖励，直接添加进背包
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            InventoryManager.Instance.inventoryUI.RefreshUI();//刷新背包
            InventoryManager.Instance.actionUI.RefreshUI();//刷新快捷栏
        }
    }
}
