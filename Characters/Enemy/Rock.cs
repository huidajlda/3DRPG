using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//石头人仍出的石头
public class Rock : MonoBehaviour
{
    public enum RockStates //石头的状态
    {
        HitPlayer,//石头砸向玩家
        HitEnemy,//玩家把石头打向敌人
        HitNothing,//扔到地上禁止的状态
    }
    private Rigidbody rb;//刚体
    public RockStates rockStates;//当前的状态
    [Header("Basic Settings")]
    public float force;//向前冲击的力
    public int damage;//石头的伤害
    public GameObject target;//目标
    private Vector3 direction;//飞行方向
    public GameObject breakEffect;//石头破碎特效
    void Start()
    {
        rb = GetComponent<Rigidbody>();//获取刚体
        rb.velocity = Vector3.one;
        rockStates =RockStates.HitPlayer;//设置最初的状态为攻击玩家
        FlyToTarget();
    }
    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude <1) 
        {
            rockStates = RockStates.HitNothing;
        }
    }
    //飞向目标
    public void FlyToTarget() 
    {
        if (target == null) //防止极限拉脱时只有动作而没有石头
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        //加Vector3.up是为了让飞行时间更长一点
        direction = (target.transform.position - transform.position+Vector3.up).normalized;//飞行方向
        //参数一力的大小和方向，参数二力的模式，这里是冲击力
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    //发生碰撞时
    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates) 
        {
            case RockStates.HitPlayer://攻击玩家
                if (other.gameObject.CompareTag("Player")) //如果碰撞的物体是玩家
                {
                    //击退玩家
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction*force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//播放眩晕动画
                    //玩家受伤
                    CharacterStats player = other.gameObject.GetComponent<CharacterStats>();
                    player.TakeDamage(damage, player);
                    rockStates = RockStates.HitNothing;
                }
                break; 
            case RockStates.HitEnemy://攻击石头人敌人
                if (other.gameObject.GetComponent<Golem>()) //判断是不是石头人
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);//造成伤害
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);//销毁石头
                }
                break;
        }
    }
}
