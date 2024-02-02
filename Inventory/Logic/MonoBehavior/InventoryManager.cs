 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//背包管理类
public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData 
    {
        public SlotHolder originalHolder;//原始的格子
        public RectTransform originalParent;//UI的位置信息
    }
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;//背包模板数据
    public InventoryData_SO inventoryData;//背包的数据
    public InventoryData_SO actionTemplate;//信息栏模板数据
    public InventoryData_SO actionData;//信息栏的数据
    public InventoryData_SO equipmentTemplate;//装备栏模板数据
    public InventoryData_SO equipmentData;//装备栏数据
    [Header("Containers")]//背包容器
    public ContainerUI inventoryUI;//背包的ui
    public ContainerUI actionUI;//信息栏的ui
    public ContainerUI equipmentUI;//装备栏的ui
    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;//正在拖拽的物体
    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;
    bool isOpen = false;
    [Header("Stats Text")]
    //TODO：后续面板需要加上面可以在这里补充
    public Text healthText;//生命面板
    public Text attackText;//攻击面板
    [Header("ToolTip")]
    public ItemToolTip toolTip;//文本描述显示
    protected override void Awake()
    {
        base.Awake();
        if(inventoryTemplate!=null)//模板不为空的话，将模板的值赋值给背包
            inventoryData=Instantiate(inventoryTemplate);
        if (actionTemplate != null)//同理
            actionData = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }
    void Start()
    {
        LoadData();//加载背包数据
        //开始时刷新一遍背包
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //按下p键开关背包
        {
            isOpen=!isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }
        UpdateStatsText(GameManager.Instance.playerStats.MaxHealth,
            (int)GameManager.Instance.playerStats.attackData.minDamage,
            (int)GameManager.Instance.playerStats.attackData.maxDamage);
    }
    //保存数据的方法
    public void SaveData() 
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }
    //读取数据的方法
    public void LoadData() 
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }
    //更新面板
    public void UpdateStatsText(int health,int minatk,int maxatk) 
    {
        healthText.text = health.ToString();
        attackText.text = minatk + "~" + maxatk;
    }
    //检查拖拽结束时物品是否在背包的任意一个Slot范围内
    public bool CheckInInventoryUI(Vector3 position) 
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++) 
        {
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) 
            {
                return true;
            }
        }
        return false;
    }
    //检查拖拽结束时物品是否在信息栏的任意一个Slot范围内
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    //检查拖拽结束时物品是否在装备栏的任意一个Slot范围内
    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)equipmentUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    //检测背包中是否具有任务物品
    public void CheckQuestItemInBag(string questItemName) 
    {
        foreach (var item in inventoryData.items) //循环背包中的物品
        {
            if (item.itemData != null) //背包不是空格子
            {
                if (item.itemData.itemName == questItemName)//是任务物品
                    //更新任务进度
                    QuestManager.Instance.UpdateQuestProgess(item.itemData.itemName, item.amount);
            }
        }
        foreach (var item in actionData.items) //循环快捷栏中的物品
        {
            if (item.itemData != null) //背包不是空格子
            {
                if (item.itemData.itemName == questItemName)//是任务物品
                    //更新任务进度
                    QuestManager.Instance.UpdateQuestProgess(item.itemData.itemName, item.amount);
            }
        }
    }
    //查找背包中的物品
    public InventoryItem QuestItemInBag(ItemData_SO questItem) 
    {
        //找到背包中和传入物品相同的物品
        return inventoryData.items.Find(i => i.itemData = questItem);
    }
    //查找快捷栏中的物品
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        //找到背包中和传入物品相同的物品
        return actionData.items.Find(i => i.itemData = questItem);
    }
}
