using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    public string questName;//���������
    [TextArea]
    public string description;//���������
    //�����״̬
    public bool isStarted;//����ʼ
    public bool isComplete;//�������(������)
    public bool isFinished;//�������(��ȡ�꽱��)
    //�����Ŀ��
    [System.Serializable]
    public class QuestRequire 
    {
        public string name;//Ŀ�������
        public int requireAmount;//Ŀ������
        public int currentAmount;//����ɵ�����
    }
    //����Ŀ����б�
    public List<QuestRequire> questRequires = new List<QuestRequire>();
    //�������б�
    public List<InventoryItem> reward =new List<InventoryItem>();
    //�����������Ƿ���ɵķ���
    public void CheckQuestProgress() 
    {
        //whereѡ���������ҷ���,��using System.Linq;����ķ���
        //���ص���һ��û����ŵ��б�������forѭ��ֻ����foreach
        //��仰���Ƿ��أ�����Ŀ������С�ڵ�������ɵ�����������Ŀ��(������˵�����Ŀ��)
        //С�ڵ�������Ϊ����һ������Ŀ����ڶ���������������ٵĻ�����ɣ�֮��ʹ�������Ŀ��������
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        //��ɵ�����Ŀ���������Ŀ���б����������ʾ��ǰ���������
        isComplete = finishRequires.Count()==questRequires.Count();
        if (isComplete) //����������
        {
            Debug.Log("wanc0");
        }
    }
    //������Ҫ ����/�ռ� ��Ŀ�����ֵ��б�
    public List<string> RequireTargetName() 
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires) 
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
    //���������轱���ķ���
    public void GiveRewards() 
    {
        foreach (var reward in reward) //���������б�
        {
            if (reward.amount < 0) //��������С��0��˵����������Ҫ�۳�����Ʒ
            {
                int requireCount = Mathf.Abs(reward.amount);//ȡ����ֵ
                //���������������Ʒ
                if (InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)
                {
                    //�������������С�ڵ�����������(˵����һ�����ڿ��������)
                    if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        //�������е������ȿ۳�
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        //�����Ǹպû��ǲ������۳�֮�󱳰��и���Ʒ��������Ӧ����0
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;
                        if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                            //��������еĻ���������ʣ����Ҫ������
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                    }
                    else//��������������㹻ֱ�Ӽ�ȥ
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount-=requireCount;
                }
                else //���������Ҳ��������Ʒ
                {
                    //��Ϊ�Ǹ������ķ������ڵ���ʱ˵�������Ѿ����,��ô����û��˵����Ʒ�ڿ��������
                    //�ڿ�����ϼ�ȥ��Ҫ�۳�����������
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount-=requireCount;
                }
            }
            else//��������0�����ǽ�����ֱ����ӽ�����
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            InventoryManager.Instance.inventoryUI.RefreshUI();//ˢ�±���
            InventoryManager.Instance.actionUI.RefreshUI();//ˢ�¿����
        }
    }
}
