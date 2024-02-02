using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public CharacterStats playerStats;
    private CinemachineFreeLook followCamera;//相机
    //继承了观察者模式接口敌人的列表
    List<IEndGameObserver> endGameObservers=new List<IEndGameObserver>();
    public void RegisterPlayer(CharacterStats player) 
    {
        Debug.Log("注册角色");
        playerStats = player;
        followCamera=FindObjectOfType<CinemachineFreeLook>();//获取相机
        if (followCamera != null) 
        {
            followCamera.Follow = playerStats.transform.GetChild(2);//设置相机跟随的目标
            followCamera.LookAt = playerStats.transform.GetChild(2);//设置相机看向的目标
        }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//过场景时不销毁此物体
    }
    //加入列表
    public void AddObserver(IEndGameObserver observer) 
    {
        endGameObservers.Add(observer); 
    }
    //移除出列表
    public void RemoveObserver(IEndGameObserver observer) 
    {
        endGameObservers.Remove(observer);
    }
    //实现广播,执行接口函数
    public void NotifyObservers() 
    {
        foreach (var observer in endGameObservers) 
        {
            observer.EndNotify();
        }
    }
    //寻找出生点的方法
    public Transform GetEntrance() 
    {
        foreach (var item in FindObjectsOfType<TransitionDestinationPoint>()) //寻找所有标签
        {
            if (item.destionationTag == TransitionDestinationPoint.DestinationTag.ENTER) //标签是出生点
            {
                return item.transform;//返回出生点
            }
        }
        return null;
    }
}
