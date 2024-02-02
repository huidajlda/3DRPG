 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//����������
public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData 
    {
        public SlotHolder originalHolder;//ԭʼ�ĸ���
        public RectTransform originalParent;//UI��λ����Ϣ
    }
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;//����ģ������
    public InventoryData_SO inventoryData;//����������
    public InventoryData_SO actionTemplate;//��Ϣ��ģ������
    public InventoryData_SO actionData;//��Ϣ��������
    public InventoryData_SO equipmentTemplate;//װ����ģ������
    public InventoryData_SO equipmentData;//װ��������
    [Header("Containers")]//��������
    public ContainerUI inventoryUI;//������ui
    public ContainerUI actionUI;//��Ϣ����ui
    public ContainerUI equipmentUI;//װ������ui
    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;//������ק������
    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;
    bool isOpen = false;
    [Header("Stats Text")]
    //TODO�����������Ҫ��������������ﲹ��
    public Text healthText;//�������
    public Text attackText;//�������
    [Header("ToolTip")]
    public ItemToolTip toolTip;//�ı�������ʾ
    protected override void Awake()
    {
        base.Awake();
        if(inventoryTemplate!=null)//ģ�岻Ϊ�յĻ�����ģ���ֵ��ֵ������
            inventoryData=Instantiate(inventoryTemplate);
        if (actionTemplate != null)//ͬ��
            actionData = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }
    void Start()
    {
        LoadData();//���ر�������
        //��ʼʱˢ��һ�鱳��
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //����p�����ر���
        {
            isOpen=!isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }
        UpdateStatsText(GameManager.Instance.playerStats.MaxHealth,
            (int)GameManager.Instance.playerStats.attackData.minDamage,
            (int)GameManager.Instance.playerStats.attackData.maxDamage);
    }
    //�������ݵķ���
    public void SaveData() 
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }
    //��ȡ���ݵķ���
    public void LoadData() 
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }
    //�������
    public void UpdateStatsText(int health,int minatk,int maxatk) 
    {
        healthText.text = health.ToString();
        attackText.text = minatk + "~" + maxatk;
    }
    //�����ק����ʱ��Ʒ�Ƿ��ڱ���������һ��Slot��Χ��
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
    //�����ק����ʱ��Ʒ�Ƿ�����Ϣ��������һ��Slot��Χ��
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
    //�����ק����ʱ��Ʒ�Ƿ���װ����������һ��Slot��Χ��
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
    //��ⱳ�����Ƿ����������Ʒ
    public void CheckQuestItemInBag(string questItemName) 
    {
        foreach (var item in inventoryData.items) //ѭ�������е���Ʒ
        {
            if (item.itemData != null) //�������ǿո���
            {
                if (item.itemData.itemName == questItemName)//��������Ʒ
                    //�����������
                    QuestManager.Instance.UpdateQuestProgess(item.itemData.itemName, item.amount);
            }
        }
        foreach (var item in actionData.items) //ѭ��������е���Ʒ
        {
            if (item.itemData != null) //�������ǿո���
            {
                if (item.itemData.itemName == questItemName)//��������Ʒ
                    //�����������
                    QuestManager.Instance.UpdateQuestProgess(item.itemData.itemName, item.amount);
            }
        }
    }
    //���ұ����е���Ʒ
    public InventoryItem QuestItemInBag(ItemData_SO questItem) 
    {
        //�ҵ������кʹ�����Ʒ��ͬ����Ʒ
        return inventoryData.items.Find(i => i.itemData = questItem);
    }
    //���ҿ�����е���Ʒ
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        //�ҵ������кʹ�����Ʒ��ͬ����Ʒ
        return actionData.items.Find(i => i.itemData = questItem);
    }
}
