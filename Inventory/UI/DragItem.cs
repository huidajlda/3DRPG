using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//需要引用命名空间
[RequireComponent(typeof(ItemUI))]//挂载时添加此脚本,确保不会报空
//继承三个接口拖拽的开始，过程及结束三个接口
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;//当前的格子里面的UI图片
    SlotHolder currentHolder;//当前的格子
    SlotHolder targetHolder;//目标格子
    void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();//获取当前UI的脚本组件
        currentHolder = GetComponentInParent<SlotHolder>();//拿到父级的UI格子脚本组件
    }
    //开始拖拽
    public void OnBeginDrag(PointerEventData eventData)//参数里面有许多信息，具体可以查看代码手册
    {
        //InventoryManager新建一个空变量用来储存原始信息
        InventoryManager.Instance.currentDrag=new InventoryManager.DragData();
        //记录原始信息,原来的格子和位置
        InventoryManager.Instance.currentDrag.originalHolder=GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //把拖动的UI设置为另一个画布的子物体，防止被其他格子挡住
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }
    //拖拽过程
    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置移动
        transform.position=eventData.position;
    }
    //结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品，交换数据
        //EventSystem自带的方法，判断给定的指针是否是UI身上，是返回true
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            if (InventoryManager.Instance.CheckInActionUI(eventData.position) ||
                InventoryManager.Instance.CheckInEquipmentUI(eventData.position) ||
                InventoryManager.Instance.CheckInInventoryUI(eventData.position)) 
            {
                //鼠标指针进入的物体是否包含SlotHolder脚本
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else 
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //Debug.Log(eventData.pointerEnter.gameObject);//有物体是返回image，格子没有物体是返回格子
                //判断是不是原来的格子
                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder) 
                {
                    switch (targetHolder.SlotType)
                    {
                        case SlotType.BAG://背包
                            SwapItem();
                            break;
                        case SlotType.WEAPON://武器
                                             //物品是武器才可以拖到武器栏
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Weapon)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ARMOR://盾牌
                                            //物品是盾牌才可以拖到防具栏,
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Armor)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ACTION://信息栏
                                             //物品是可使用的才可以拖到信息栏
                            if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Useable)
                            {
                                SwapItem();
                            }
                            break;
                    }
                }
                currentHolder.UpdateItem();//刷新原来的背包
                targetHolder.UpdateItem();//刷新目标背包
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);//将格子父级设置回去
        RectTransform t=transform as RectTransform;
        t.offsetMax = -Vector2.one*5;
        t.offsetMin = Vector2.one*5;
    }
    //物品的交换
    public void SwapItem() 
    {
        //拿到交换目标所以那个背包列表的哪个UI物品
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.index];
        //正在拖拽的物品
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.index];
        bool isSameItem=tempItem.itemData==targetItem.itemData;//物品是否相同
        if (isSameItem && targetItem.itemData.stackable) //物品相同且是可以堆叠的
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else 
        {
            //交换
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.index]= targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.index] = tempItem;
        }
    }
}
