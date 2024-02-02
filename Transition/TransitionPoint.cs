using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,//同场景传送
        DifferentScene,//不是同场景传送
    }
    [Header("Transition Info")]//传送的信息
    public string sceneName;//场景的名字
    public TransitionType transitionType;//枚举遍历(判断是不是同场景传送)
    public TransitionDestinationPoint.DestinationTag destinationTag;//终点标签类型
    private bool canTrans;//玩家触发时才可以传送
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans) 
        {
            Debug.Log("xx");
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    //触发过程
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) //是玩家才可以传送
        {
            canTrans = true;
        }
    }
    //触发离开
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) //离开设置成不能传送
        {
            canTrans = false;
        }
    }
}
