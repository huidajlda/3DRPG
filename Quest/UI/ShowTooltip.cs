using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;//������Ʒ������
    void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();//��ȡ����
    }
    //������
    public void OnPointerEnter(PointerEventData eventData)
    {
        //����tooltip��UI
        QuestUI.Instance.toolTip.gameObject.SetActive(true);
        //����tooltip����д�õķ���
        QuestUI.Instance.toolTip.SetupToolTtip(currentItemUI.currentItemData);
    }
    //����뿪
    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(false);
    }
}
