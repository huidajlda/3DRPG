using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//��ť����ʾ���ı�
    public QuestData_SO currentData;//��ǰ��������
    public Text questContentText;//��������
    void Awake()
    {
        //��ӵ���¼�
        GetComponent<Button>().onClick.AddListener(updateQuestContent);
    }
    //��������ť����
    public void SetupNameButton(QuestData_SO questData) 
    {
        currentData = questData;
        if (questData.isComplete)//������ɾ���ʾ���������+���
            questNameText.text = questData.questName + "(���)";
        else
            questNameText.text = questData.questName;//û��ɾ���ʾ��������
    }
    //��������ť,��ʾ������ϸ��Ϣ
    void updateQuestContent() 
    {
        questContentText.text = currentData.description;//�������������ı�
        QuestUI.Instance.SetupRequireList(currentData);//��ʾ�������
        foreach (Transform item in QuestUI.Instance.rewardTransform) 
        {//ɾ����һ������Ľ���
            Destroy(item.gameObject);
        }
        foreach (var item in currentData.reward) //��ʾ�����б��е�ÿ������
        {
            QuestUI.Instance.SetupRewardItem(item.itemData, item.amount);
        }
    }

}
