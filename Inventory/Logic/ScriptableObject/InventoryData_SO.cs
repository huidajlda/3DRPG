using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//背包类
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items=new List<InventoryItem>();
    //物品添加进背包
    public void AddItem(ItemData_SO newItemData,int amount) 
    {
        bool found = false;//背包中是否有此物品
        if (newItemData.stackable) //判断是否可以堆叠
        {
            foreach (var item in items) //循环背包列表
            {
                if (item.itemData == newItemData) 
                {
                    item.amount += amount;//背包中的数量加上拾取的数量
                    found = true;//背包中有此物品
                    break;
                }
            }
        }
        for (int i = 0; i < items.Count; i++) //寻找列表中的空位置
        {
            if (items[i].itemData == null && !found) //为空且背包中没有此物品
            {
                //添加物品进列表
                items[i].itemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }
    }
}
//背包中保存的物品类 
[System.Serializable]
public class InventoryItem 
{
    public ItemData_SO itemData;//物品
    public int amount;//数量
}
