using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType //格子类型
{
    BAG,//背包格
    WEAPON,//武器格
    ARMOR,//盾牌格
    ACTION
}
//IPointerClickHandler接口可以接受鼠标点击的回调
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType SlotType;//格子的类型
    public ItemUI itemUI;//显示的物品的UI信息
    //双击使用格子里可使用物品的方法
    public void OnPointerClick(PointerEventData eventData)
    {
        //eventData.clickCount可以判断鼠标点击次数
        if (eventData.clickCount % 2 == 0) 
        {
            UseItem();//调用使用物品的方法
        }
    }
    //使用物品的方法
    public void UseItem() 
    {
        if (itemUI.GetItem() != null) //格子物品不为空
        {
            //物品是可使用的且数量大于0
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.index].amount > 0)
            {
                //加血
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);
                itemUI.Bag.items[itemUI.index].amount -= 1;//数量减一
                //检测任务物品更新进度
                QuestManager.Instance.UpdateQuestProgess(itemUI.GetItem().name, -1);
            }
            //更新背包
            UpdateItem();
        }
    }
    //更新格子里的物品
    public void UpdateItem()
    {
        switch (SlotType) 
        {
            case SlotType.BAG://如果是背包格子
                itemUI.Bag = InventoryManager.Instance.inventoryData;//将背包数据赋值给Bag
                break; 
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//将装备栏数据赋值给Bag
                //装备(切换)武器
                if (itemUI.Bag.items[itemUI.index].itemData != null)
                {
                    //调用切换武器的方法
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.index].itemData);
                }
                else //为空时卸载掉武器(比如耐久为0，但这里没做)
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break; 
            case SlotType.ARMOR:
                //TODO：盾牌的做法和武器类似
                itemUI.Bag = InventoryManager.Instance.equipmentData;//将装备栏数据赋值给Bag
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;//将信息栏数据赋值给Bag
                break;
        }
        //获取格子所在的背包里面物品的数据
        var item = itemUI.Bag.items[itemUI.index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }
    //鼠标悬停在物体上时
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem()) //是否存在物体
        {
            //设置物体的文本信息
            InventoryManager.Instance.toolTip.SetupToolTtip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);//显示UI
        }
    }
    //鼠标离开时
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);//隐藏UI
    }
    void OnDisable()
    {
        //鼠标悬停在物体上，然后关闭背包，就会导致文本描述不消失
        //所以在背包关闭时也要隐藏UI
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);//隐藏UI
    }
}
