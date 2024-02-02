using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items=new List<InventoryItem>();
    //��Ʒ��ӽ�����
    public void AddItem(ItemData_SO newItemData,int amount) 
    {
        bool found = false;//�������Ƿ��д���Ʒ
        if (newItemData.stackable) //�ж��Ƿ���Զѵ�
        {
            foreach (var item in items) //ѭ�������б�
            {
                if (item.itemData == newItemData) 
                {
                    item.amount += amount;//�����е���������ʰȡ������
                    found = true;//�������д���Ʒ
                    break;
                }
            }
        }
        for (int i = 0; i < items.Count; i++) //Ѱ���б��еĿ�λ��
        {
            if (items[i].itemData == null && !found) //Ϊ���ұ�����û�д���Ʒ
            {
                //�����Ʒ���б�
                items[i].itemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }
    }
}
//�����б������Ʒ�� 
[System.Serializable]
public class InventoryItem 
{
    public ItemData_SO itemData;//��Ʒ
    public int amount;//����
}
