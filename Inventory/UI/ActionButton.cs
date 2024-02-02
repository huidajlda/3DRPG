using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;//���ÿ�ݰ���
    private SlotHolder currentSlotHolder;
    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }
    void Update()
    {
        //������1~6������һ�����ҿ��������������
        if (Input.GetKeyDown(actionKey)&&currentSlotHolder.itemUI.GetItem()) 
        {
            currentSlotHolder.UseItem();
        }
    }
}
