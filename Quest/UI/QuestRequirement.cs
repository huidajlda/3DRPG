using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;//����Ҫ����ı�
    private Text progressNumber;//������ȵ��ı�
    void Awake()
    {
        //�����������Լ����ϵ�����������壬����ֱ��Awake���
        requireName = GetComponent<Text>();
        progressNumber =transform.GetChild(0).GetComponent<Text>();
    }
    //��������Ҫ��ͽ����ı��ķ���
    public void SetupRequirement(string name, int amount, int currentAmount) 
    {
        requireName.text = name;//��������Ҫ����ı�
        progressNumber.text=currentAmount.ToString()+" / "+amount.ToString();
    }
    //������ɺ�(��ȡ�꽱��)�޸Ľ����ı�,���ط���
    public void SetupRequirement(string name, bool isFinished) 
    {
        if (isFinished) 
        {
            requireName.text = name;//��������Ҫ����ı�
            progressNumber.text = "���";
            requireName.color = Color.gray;//��������ɫ���
            progressNumber.color=Color.gray;//��������ɫ���
        }
    }
}
