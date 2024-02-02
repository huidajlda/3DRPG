using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("����")]
    public float kickForce=10;//���ɵ���

    public void KickOff() 
    {
        if (attackTarget != null) 
        {
            transform.LookAt(attackTarget.transform);
            //���˷���
            Vector3 direction=attackTarget.transform.position-transform.position;
            direction.Normalize();
            //����ƶ�
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            //���˷���*������
            attackTarget.GetComponent<NavMeshAgent>().velocity =direction*kickForce;//���˵��ٶ�
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");//�������ѣ�ζ���
        }
    }
}
