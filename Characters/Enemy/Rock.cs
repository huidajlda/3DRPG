using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//ʯͷ���Գ���ʯͷ
public class Rock : MonoBehaviour
{
    public enum RockStates //ʯͷ��״̬
    {
        HitPlayer,//ʯͷ�������
        HitEnemy,//��Ұ�ʯͷ�������
        HitNothing,//�ӵ����Ͻ�ֹ��״̬
    }
    private Rigidbody rb;//����
    public RockStates rockStates;//��ǰ��״̬
    [Header("Basic Settings")]
    public float force;//��ǰ�������
    public int damage;//ʯͷ���˺�
    public GameObject target;//Ŀ��
    private Vector3 direction;//���з���
    public GameObject breakEffect;//ʯͷ������Ч
    void Start()
    {
        rb = GetComponent<Rigidbody>();//��ȡ����
        rb.velocity = Vector3.one;
        rockStates =RockStates.HitPlayer;//���������״̬Ϊ�������
        FlyToTarget();
    }
    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude <1) 
        {
            rockStates = RockStates.HitNothing;
        }
    }
    //����Ŀ��
    public void FlyToTarget() 
    {
        if (target == null) //��ֹ��������ʱֻ�ж�����û��ʯͷ
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        //��Vector3.up��Ϊ���÷���ʱ�����һ��
        direction = (target.transform.position - transform.position+Vector3.up).normalized;//���з���
        //����һ���Ĵ�С�ͷ��򣬲���������ģʽ�������ǳ����
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    //������ײʱ
    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates) 
        {
            case RockStates.HitPlayer://�������
                if (other.gameObject.CompareTag("Player")) //�����ײ�����������
                {
                    //�������
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction*force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//����ѣ�ζ���
                    //�������
                    CharacterStats player = other.gameObject.GetComponent<CharacterStats>();
                    player.TakeDamage(damage, player);
                    rockStates = RockStates.HitNothing;
                }
                break; 
            case RockStates.HitEnemy://����ʯͷ�˵���
                if (other.gameObject.GetComponent<Golem>()) //�ж��ǲ���ʯͷ��
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);//����˺�
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);//����ʯͷ
                }
                break;
        }
    }
}
