using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    //让基础的任务数据不被修改,修改的是QuestTask类里面的任务数据
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;//保存任务数据
        //三个任务的状态
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }
    //任务列表
    public List<QuestTask> tasks = new List<QuestTask>();
    void Start()
    {
        LoadQuestManager();//加载数据
    }
    //判断任务列表中是否已经有某个任务了
    public bool HaveQuest(QuestData_SO data)
    {
        //寻常方法遍历列表看任务名字和传进来的任务数据的名字是否相同
        //新方法:引用命名空间using System.Linq;
        //通常使用在列表或数组中,能够帮助循环查找到想要的东西
        //Any是其提供的方法，返回bool，里面传一个lamdom表达式，q表示的就是tasks里面的一个数据
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);
        else return false;
    }
    //查找列表里面的任务数据
    public QuestTask GetTask(QuestData_SO data)
    {
        //这也是Linq里面的方法，返回的是满足条件q的变量
        return tasks.Find(q => q.questData.questName == data.questName);
    }
    //检测任务进度的方法（进度名，数量）
    //调用:敌人死亡,拾取物品
    public void UpdateQuestProgess(string requireName, int amount)
    {
        foreach (var task in tasks) //遍历整个任务列表
        {
            if (task.IsFinished)
                continue;//如果该任务结束了，则跳过
            //找到任务列表中和传入名字相同的任务进度(Find是using System.Linq;的方法)
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null) //不为空，说明有匹配的任务进度
                matchTask.currentAmount += amount;//当前进度加上传进来的进度
            task.questData.CheckQuestProgress();//检测任务有没有完成
        }
    }
    //保存任务数据
    public void SaveQuestSystem()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);//保存任务数量
        for (int i = 0; i < tasks.Count; i++)
        {
            //这里不能直接保存tasks的原因时，tasks时列表不算是Object，而tasks里面保存的Data数据是Object
            //而保存函数的第一个参数是要Object，第二个参数是取的名字。
            SaveManager.Instance.Save(tasks[i].questData, "task" + i);
        }
    }
    //加载任务数据
    public void LoadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");//拿到保存的任务数量
        //循环读取任务数据，重新生成QuestTask然后加到tasks列表中
        for (int i = 0; i < questCount; i++) 
        {
            var newquest=ScriptableObject.CreateInstance<QuestData_SO>();//生成一个空任务数据的实例
            //按保存的task需要读取数据
            SaveManager.Instance.Load(newquest, "task" + i);//读取保存的数据，将空数据覆盖掉
            tasks.Add(new QuestTask { questData = newquest });//新创建QuestTask，并初始化里面的任务数据，添加进列表
        }
    }
}
