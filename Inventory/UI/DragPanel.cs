using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragPanel : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //物体的中心+鼠标的增量
        //delta每一帧产生的位移,在拖拽时会实时更新
        //canvas.scaleFactor是缩放画布使其适合屏幕
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    //鼠标按下
    public void OnPointerDown(PointerEventData eventData)
    {
        //将鼠标按下(拖拽)的那个，放到下面（unity渲染顺序是在同一个父级下，越靠后就越先渲染）
        rectTransform.SetSiblingIndex(2);
    }
}
