using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemToolTip toolTip;//��Ʒ�����ı��Ľű���������Ƶ���Ʒ����ʾ��Ʒ�����Ľű���
    bool isOpen;//�Ƿ��
    [Header("����ť")]
    public RectTransform questListTransform;//�����б����������ť���ɵ�λ��
    public QuestNameButton questNameButton;//���ɵİ�ť��Ԥ����
    [Header("�ı�����")]
    public Text questContentText;//��������
    [Header("�������")]
    public RectTransform requireTransform;//����������ɵ�λ��
    public QuestRequirement requirment;//������ȵ��ı����ݽű�
    [Header("��������")]
    public RectTransform rewardTransform;//���ɽ������ӵ�λ��
    public ItemUI rewardUI;//��������
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //����T��������UI
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);//��ʾ��ʧ��
            questContentText.text = "";//����������ڿգ��մ򿪲���ʾ
            //��ʾ�����б��������
            SetupQuestList();
            if (!isOpen) //û�д��������ʾ�ر�
            {
                toolTip.gameObject.SetActive(false);
            }
        }
    }
    //��ʾ�����б�������ݵķ���
    public void SetupQuestList() 
    {
        //�ȱ��������б�İ�ť����������
        foreach (Transform item in questListTransform) 
        {
            Destroy(item.gameObject);
        }
        //�������������ӣ���������
        foreach (Transform item in rewardTransform) 
        {
            Destroy(item.gameObject);
        }
        //�����������ʾҲ�������������ʱ������ԭ���Ķ���
        foreach (Transform item in requireTransform) 
        {
            Destroy(item.gameObject);
        }
        //���������б��������������ť
        foreach (var task in QuestManager.Instance.tasks) 
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupNameButton(task.questData);//���ð�ť���������ı��ķ���
            newTask.questContentText=questContentText;//������������
        }
    }
    //��ʾ������ȵķ���
    public void SetupRequireList(QuestData_SO questData) 
    {
        //��֤������������µ�
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        //�����������
        foreach (var require in questData.questRequires) 
        {
            var q = Instantiate(requirment, requireTransform);
            if (questData.isFinished)//������������
                q.SetupRequirement(require.name, questData.isFinished);//����������������ı��ķ���������
            else//����������������ı��ķ���
                q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);
        }
    }
    //��ʾ������Ʒ
    public void SetupRewardItem(ItemData_SO itemData,int amount) 
    {
        var item = Instantiate(rewardUI, rewardTransform);//���ɽ���
        item.SetupItemUI(itemData, amount);//���ý���ͼƬ������
    }
}
