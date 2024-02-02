using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;//任务要求的文本
    private Text progressNumber;//任务进度的文本
    void Awake()
    {
        //这两个就是自己身上的组件或子物体，所以直接Awake获得
        requireName = GetComponent<Text>();
        progressNumber =transform.GetChild(0).GetComponent<Text>();
    }
    //设置任务要求和进度文本的方法
    public void SetupRequirement(string name, int amount, int currentAmount) 
    {
        requireName.text = name;//设置任务要求的文本
        progressNumber.text=currentAmount.ToString()+" / "+amount.ToString();
    }
    //任务完成后(领取完奖励)修改进度文本,重载方法
    public void SetupRequirement(string name, bool isFinished) 
    {
        if (isFinished) 
        {
            requireName.text = name;//设置任务要求的文本
            progressNumber.text = "完成";
            requireName.color = Color.gray;//将字体颜色变灰
            progressNumber.color=Color.gray;//将字体颜色变灰
        }
    }
}
