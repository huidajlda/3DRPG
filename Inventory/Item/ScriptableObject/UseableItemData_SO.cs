using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Useable Item",menuName ="Inventory/Useable Item Data")]
public class UseableItemData_SO : ScriptableObject
{
    //TODO:所有在使用物品后想要改变的值都写在这
    public int healthPoint;//暂时只做加血
}
