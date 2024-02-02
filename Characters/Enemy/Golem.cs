using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("技能")]
    public float kickForce = 25;//击飞的力
    public GameObject rockPrefab;//扔出的石头的预制体
    public Transform handPos;//扔出手的位置

    //石头人的击退切造成伤害的方法
    public void KickOff() 
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform)) //目标不为空才造成伤害
        {
            //获取目标身上的角色属性脚本
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            //击退的方向
            Vector3 direction=attackTarget.transform.position - transform.position;
            direction.Normalize();
            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction* kickForce;
            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");//播放眩晕动画
            Debug.Log("造成伤害");
            targetStats.TakeDamage(characterStats, targetStats);//调用造成伤害的方法
        }
    }
    //扔石头
    public void ThrowRock() 
    {
        if (attackTarget != null) 
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);//实例化石头
            rock.GetComponent<Rock>().target = attackTarget;//给石头设置目标
        }
    }
}
