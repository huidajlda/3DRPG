using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;//显示的图片
    public Text amount=null;//物品数量
    public ItemData_SO currentItemData;//当前物品的数据
    public InventoryData_SO Bag { get; set; }//背包
    public int index { get; set; } = -1;//物品在背包里面的序号,初始值为-1
    //设置背包图片和数量的方法
    public void SetupItemUI(ItemData_SO item,int itemAmount) 
    {
        if (itemAmount == 0) 
        {
            //数量为0时将数据从背包中移除
            Bag.items[index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }
        //如果数量小于0，就是任务上需要提交的任务目标的数量，这是不需要生成的
        if (itemAmount < 0) 
            item=null;
        if (item != null)
        {
            currentItemData = item;//保存当前物品数据
            icon.sprite = item.itemIcon;//设置图片显示
            amount.text = itemAmount.ToString();//设置数量
            icon.gameObject.SetActive(true);//激活物体
        }
        else //如果物品为空,不激活这个格子
        {
            icon.gameObject.SetActive(false);//失活物体
        }
    }
    //获取当前UI对应物品的方法
    public ItemData_SO GetItem() 
    {
        return Bag.items[index].itemData;
    }
}
