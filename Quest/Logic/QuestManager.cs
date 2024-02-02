using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    //�û������������ݲ����޸�,�޸ĵ���QuestTask���������������
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;//������������
        //���������״̬
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }
    //�����б�
    public List<QuestTask> tasks = new List<QuestTask>();
    void Start()
    {
        LoadQuestManager();//��������
    }
    //�ж������б����Ƿ��Ѿ���ĳ��������
    public bool HaveQuest(QuestData_SO data)
    {
        //Ѱ�����������б��������ֺʹ��������������ݵ������Ƿ���ͬ
        //�·���:���������ռ�using System.Linq;
        //ͨ��ʹ�����б��������,�ܹ�����ѭ�����ҵ���Ҫ�Ķ���
        //Any�����ṩ�ķ���������bool�����洫һ��lamdom���ʽ��q��ʾ�ľ���tasks�����һ������
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);
        else return false;
    }
    //�����б��������������
    public QuestTask GetTask(QuestData_SO data)
    {
        //��Ҳ��Linq����ķ��������ص�����������q�ı���
        return tasks.Find(q => q.questData.questName == data.questName);
    }
    //���������ȵķ�������������������
    //����:��������,ʰȡ��Ʒ
    public void UpdateQuestProgess(string requireName, int amount)
    {
        foreach (var task in tasks) //�������������б�
        {
            if (task.IsFinished)
                continue;//�������������ˣ�������
            //�ҵ������б��кʹ���������ͬ���������(Find��using System.Linq;�ķ���)
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null) //��Ϊ�գ�˵����ƥ����������
                matchTask.currentAmount += amount;//��ǰ���ȼ��ϴ������Ľ���
            task.questData.CheckQuestProgress();//���������û�����
        }
    }
    //������������
    public void SaveQuestSystem()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);//������������
        for (int i = 0; i < tasks.Count; i++)
        {
            //���ﲻ��ֱ�ӱ���tasks��ԭ��ʱ��tasksʱ�б�����Object����tasks���汣���Data������Object
            //�����溯���ĵ�һ��������ҪObject���ڶ���������ȡ�����֡�
            SaveManager.Instance.Save(tasks[i].questData, "task" + i);
        }
    }
    //������������
    public void LoadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");//�õ��������������
        //ѭ����ȡ�������ݣ���������QuestTaskȻ��ӵ�tasks�б���
        for (int i = 0; i < questCount; i++) 
        {
            var newquest=ScriptableObject.CreateInstance<QuestData_SO>();//����һ�����������ݵ�ʵ��
            //�������task��Ҫ��ȡ����
            SaveManager.Instance.Load(newquest, "task" + i);//��ȡ��������ݣ��������ݸ��ǵ�
            tasks.Add(new QuestTask { questData = newquest });//�´���QuestTask������ʼ��������������ݣ���ӽ��б�
        }
    }
}
