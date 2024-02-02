using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public CharacterStats playerStats;
    private CinemachineFreeLook followCamera;//���
    //�̳��˹۲���ģʽ�ӿڵ��˵��б�
    List<IEndGameObserver> endGameObservers=new List<IEndGameObserver>();
    public void RegisterPlayer(CharacterStats player) 
    {
        Debug.Log("ע���ɫ");
        playerStats = player;
        followCamera=FindObjectOfType<CinemachineFreeLook>();//��ȡ���
        if (followCamera != null) 
        {
            followCamera.Follow = playerStats.transform.GetChild(2);//������������Ŀ��
            followCamera.LookAt = playerStats.transform.GetChild(2);//������������Ŀ��
        }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//������ʱ�����ٴ�����
    }
    //�����б�
    public void AddObserver(IEndGameObserver observer) 
    {
        endGameObservers.Add(observer); 
    }
    //�Ƴ����б�
    public void RemoveObserver(IEndGameObserver observer) 
    {
        endGameObservers.Remove(observer);
    }
    //ʵ�ֹ㲥,ִ�нӿں���
    public void NotifyObservers() 
    {
        foreach (var observer in endGameObservers) 
        {
            observer.EndNotify();
        }
    }
    //Ѱ�ҳ�����ķ���
    public Transform GetEntrance() 
    {
        foreach (var item in FindObjectsOfType<TransitionDestinationPoint>()) //Ѱ�����б�ǩ
        {
            if (item.destionationTag == TransitionDestinationPoint.DestinationTag.ENTER) //��ǩ�ǳ�����
            {
                return item.transform;//���س�����
            }
        }
        return null;
    }
}
