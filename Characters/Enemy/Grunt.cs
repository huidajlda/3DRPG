using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("技能")]
    public float kickForce=10;//击飞的力

    public void KickOff() 
    {
        if (attackTarget != null) 
        {
            transform.LookAt(attackTarget.transform);
            //击退方向
            Vector3 direction=attackTarget.transform.position-transform.position;
            direction.Normalize();
            //打断移动
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            //击退方向*击退力
            attackTarget.GetComponent<NavMeshAgent>().velocity =direction*kickForce;//击退的速度
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");//播放玩家眩晕动画
        }
    }
}
