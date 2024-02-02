using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType //物品类型
{
    Useable,//可使用的，比如血瓶
    Weapon,//武器，剑
    Armor//装备
}
//可以在菜单上创建物品属性
[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;//物品类型
    public string itemName;//物品名称
    public Sprite itemIcon;//物品在背包中的图片
    public int itemAmount;//数量
    public bool stackable;//是否可以堆叠
    [Header("武器信息")]
    public GameObject weaponPrefab;//武器的预设体
    public AttackData_SO weaponData;//武器的攻击数据
    [Header("可使用物品的增益数据")]
    public UseableItemData_SO useableData;
    [Header("武器动画")]
    //武器的动画
    public AnimatorOverrideController weaponAnimator;
    [TextArea]//让字符串的输入是文本框
    public string description="";//物品详情,默认为空
}
