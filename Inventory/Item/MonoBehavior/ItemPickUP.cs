using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUP : MonoBehaviour
{
    public ItemData_SO itemData;//��Ʒ��������Ϣ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //�ǲ�����Ҵ���
        {
            //TODO:����Ʒ��ӵ�����
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            //ʰȡ��Ʒ��ˢ�±��� 
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //װ������
            //GameManager.Instance.playerStats.EquipWeapon(itemData);//����װ�������ķ���
            //����Ƿ�������
            QuestManager.Instance.UpdateQuestProgess(itemData.itemName, itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
