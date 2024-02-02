using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//��Ҫ���������ռ�
[RequireComponent(typeof(ItemUI))]//����ʱ��Ӵ˽ű�,ȷ�����ᱨ��
//�̳������ӿ���ק�Ŀ�ʼ�����̼����������ӿ�
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;//��ǰ�ĸ��������UIͼƬ
    SlotHolder currentHolder;//��ǰ�ĸ���
    SlotHolder targetHolder;//Ŀ�����
    void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();//��ȡ��ǰUI�Ľű����
        currentHolder = GetComponentInParent<SlotHolder>();//�õ�������UI���ӽű����
    }
    //��ʼ��ק
    public void OnBeginDrag(PointerEventData eventData)//���������������Ϣ��������Բ鿴�����ֲ�
    {
        //InventoryManager�½�һ���ձ�����������ԭʼ��Ϣ
        InventoryManager.Instance.currentDrag=new InventoryManager.DragData();
        //��¼ԭʼ��Ϣ,ԭ���ĸ��Ӻ�λ��
        InventoryManager.Instance.currentDrag.originalHolder=GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //���϶���UI����Ϊ��һ�������������壬��ֹ���������ӵ�ס
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }
    //��ק����
    public void OnDrag(PointerEventData eventData)
    {
        //�������λ���ƶ�
        transform.position=eventData.position;
    }
    //������ק
    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ����������
        //EventSystem�Դ��ķ������жϸ�����ָ���Ƿ���UI���ϣ��Ƿ���true
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            if (InventoryManager.Instance.CheckInActionUI(eventData.position) ||
                InventoryManager.Instance.CheckInEquipmentUI(eventData.position) ||
                InventoryManager.Instance.CheckInInventoryUI(eventData.position)) 
            {
                //���ָ�����������Ƿ����SlotHolder�ű�
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else 
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //Debug.Log(eventData.pointerEnter.gameObject);//�������Ƿ���image������û�������Ƿ��ظ���
                //�ж��ǲ���ԭ���ĸ���
                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder) 
                {
                    switch (targetHolder.SlotType)
                    {
                        case SlotType.BAG://����
                            SwapItem();
                            break;
                        case SlotType.WEAPON://����
                                             //��Ʒ�������ſ����ϵ�������
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Weapon)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ARMOR://����
                                            //��Ʒ�Ƕ��Ʋſ����ϵ�������,
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Armor)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ACTION://��Ϣ��
                                             //��Ʒ�ǿ�ʹ�õĲſ����ϵ���Ϣ��
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Useable)
                            {
                                SwapItem();
                            }
                            break;
                    }
                }
                currentHolder.UpdateItem();//ˢ��ԭ���ı���
                targetHolder.UpdateItem();//ˢ��Ŀ�걳��
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);//�����Ӹ������û�ȥ
        RectTransform t=transform as RectTransform;
        t.offsetMax = -Vector2.one*5;
        t.offsetMin = Vector2.one*5;
    }
    //��Ʒ�Ľ���
    public void SwapItem() 
    {
        //�õ�����Ŀ�������Ǹ������б���ĸ�UI��Ʒ
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.index];
        //������ק����Ʒ
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.index];
        bool isSameItem=tempItem.itemData==targetItem.itemData;//��Ʒ�Ƿ���ͬ
        if (isSameItem && targetItem.itemData.stackable) //��Ʒ��ͬ���ǿ��Զѵ���
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else 
        {
            //����
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.index]= targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.index] = tempItem;
        }
    }
}
