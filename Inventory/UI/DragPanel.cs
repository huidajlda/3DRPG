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
        //���������+��������
        //deltaÿһ֡������λ��,����קʱ��ʵʱ����
        //canvas.scaleFactor�����Ż���ʹ���ʺ���Ļ
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    //��갴��
    public void OnPointerDown(PointerEventData eventData)
    {
        //����갴��(��ק)���Ǹ����ŵ����棨unity��Ⱦ˳������ͬһ�������£�Խ�����Խ����Ⱦ��
        rectTransform.SetSiblingIndex(2);
    }
}
