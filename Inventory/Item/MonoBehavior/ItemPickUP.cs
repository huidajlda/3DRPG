using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUP : MonoBehaviour
{
    public ItemData_SO itemData;//物品的属性信息
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //是不是玩家触发
        {
            //TODO:将物品添加到背包
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            //拾取物品后刷新背包 
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备武器
            //GameManager.Instance.playerStats.EquipWeapon(itemData);//调用装备武器的方法
            //检测是否有任务
            QuestManager.Instance.UpdateQuestProgess(itemData.itemName, itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
