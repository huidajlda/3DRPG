using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType //��������
{
    BAG,//������
    WEAPON,//������
    ARMOR,//���Ƹ�
    ACTION
}
//IPointerClickHandler�ӿڿ��Խ���������Ļص�
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType SlotType;//���ӵ�����
    public ItemUI itemUI;//��ʾ����Ʒ��UI��Ϣ
    //˫��ʹ�ø������ʹ����Ʒ�ķ���
    public void OnPointerClick(PointerEventData eventData)
    {
        //eventData.clickCount�����ж����������
        if (eventData.clickCount % 2 == 0) 
        {
            UseItem();//����ʹ����Ʒ�ķ���
        }
    }
    //ʹ����Ʒ�ķ���
    public void UseItem() 
    {
        if (itemUI.GetItem() != null) //������Ʒ��Ϊ��
        {
            //��Ʒ�ǿ�ʹ�õ�����������0
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.index].amount > 0)
            {
                //��Ѫ
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);
                itemUI.Bag.items[itemUI.index].amount -= 1;//������һ
                //���������Ʒ���½���
                QuestManager.Instance.UpdateQuestProgess(itemUI.GetItem().name, -1);
            }
            //���±���
            UpdateItem();
        }
    }
    //���¸��������Ʒ
    public void UpdateItem()
    {
        switch (SlotType) 
        {
            case SlotType.BAG://����Ǳ�������
                itemUI.Bag = InventoryManager.Instance.inventoryData;//���������ݸ�ֵ��Bag
                break; 
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//��װ�������ݸ�ֵ��Bag
                //װ��(�л�)����
                if (itemUI.Bag.items[itemUI.index].itemData != null)
                {
                    //�����л������ķ���
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.index].itemData);
                }
                else //Ϊ��ʱж�ص�����(�����;�Ϊ0��������û��)
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break; 
            case SlotType.ARMOR:
                //TODO�����Ƶ���������������
                itemUI.Bag = InventoryManager.Instance.equipmentData;//��װ�������ݸ�ֵ��Bag
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;//����Ϣ�����ݸ�ֵ��Bag
                break;
        }
        //��ȡ�������ڵı���������Ʒ������
        var item = itemUI.Bag.items[itemUI.index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }
    //�����ͣ��������ʱ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem()) //�Ƿ��������
        {
            //����������ı���Ϣ
            InventoryManager.Instance.toolTip.SetupToolTtip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);//��ʾUI
        }
    }
    //����뿪ʱ
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);//����UI
    }
    void OnDisable()
    {
        //�����ͣ�������ϣ�Ȼ��رձ������ͻᵼ���ı���������ʧ
        //�����ڱ����ر�ʱҲҪ����UI
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);//����UI
    }
}
