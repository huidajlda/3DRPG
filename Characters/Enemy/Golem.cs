using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("����")]
    public float kickForce = 25;//���ɵ���
    public GameObject rockPrefab;//�ӳ���ʯͷ��Ԥ����
    public Transform handPos;//�ӳ��ֵ�λ��

    //ʯͷ�˵Ļ���������˺��ķ���
    public void KickOff() 
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform)) //Ŀ�겻Ϊ�ղ�����˺�
        {
            //��ȡĿ�����ϵĽ�ɫ���Խű�
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            //���˵ķ���
            Vector3 direction=attackTarget.transform.position - transform.position;
            direction.Normalize();
            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction* kickForce;
            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");//����ѣ�ζ���
            Debug.Log("����˺�");
            targetStats.TakeDamage(characterStats, targetStats);//��������˺��ķ���
        }
    }
    //��ʯͷ
    public void ThrowRock() 
    {
        if (attackTarget != null) 
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);//ʵ����ʯͷ
            rock.GetComponent<Rock>().target = attackTarget;//��ʯͷ����Ŀ��
        }
    }
}
