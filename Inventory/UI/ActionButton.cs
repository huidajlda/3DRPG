using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;//设置快捷按键
    private SlotHolder currentSlotHolder;
    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }
    void Update()
    {
        //按下了1~6的其中一个键且快捷栏里面有数据
        if (Input.GetKeyDown(actionKey)&&currentSlotHolder.itemUI.GetItem()) 
        {
            currentSlotHolder.UseItem();
        }
    }
}
