using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;//挂载物品的数据
    void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();//获取数据
    }
    //鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        //激活tooltip的UI
        QuestUI.Instance.toolTip.gameObject.SetActive(true);
        //调用tooltip里面写好的方法
        QuestUI.Instance.toolTip.SetupToolTtip(currentItemUI.currentItemData);
    }
    //鼠标离开
    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(false);
    }
}
