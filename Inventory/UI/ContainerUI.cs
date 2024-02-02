using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;//格子数组
    //刷新背包UI界面
    public void RefreshUI() 
    {
        for (int i = 0; i < slotHolders.Length; i++) 
        {
            slotHolders[i].itemUI.index = i;//将格子的序号赋值
            slotHolders[i].UpdateItem();
        }
    }
}
