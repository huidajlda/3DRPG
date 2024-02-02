using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//��ɫ���ƽű�
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;//��������
    private Animator animator;//����������
    private GameObject attackTarget;//����Ŀ��
    private float lastAttackTime;//�������
    private CharacterStats characterStats;//��ɫ����
    private bool isDead;//�Ƿ�����
    private float stopDistance;//ֹͣ���루�������룩
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//��ȡ�����������
        animator = GetComponent<Animator>();//��ȡ����������
        characterStats = GetComponent<CharacterStats>();//��ȡ��ɫ���ԵĽű�
        stopDistance = agent.stoppingDistance;
    }
    void OnEnable()//����ʱ����¼�ע��
    {
        oneMouseManager.Instance.OnMouseClicked += MoveToTarget;//�������ƶ�������ӵ��¼�
        oneMouseManager.Instance.OnEnemyClick += MoveToAttackTarget;
        GameManager.Instance.RegisterPlayer(characterStats);//ע�����
    }
    void Start()
    {
        SaveManager.Instance.LoadPlayerData();//�����������
    }
    void OnDisable()//����ʱȡ���¼�����
    {
        Debug.Log("������");
        oneMouseManager.Instance.OnMouseClicked -= MoveToTarget;
        oneMouseManager.Instance.OnEnemyClick -= MoveToAttackTarget;
    }
    void Update()
    {
        if (isDead) 
        {
            GameManager.Instance.NotifyObservers();//���ͽ�����Ϸ�Ĺ㲥
        }
        isDead = characterStats.CurrentHealth == 0;
        SwitchAnimation();
        lastAttackTime-=Time.deltaTime;
    }
    //���ƶ������л�
    private void SwitchAnimation() 
    {
        //���ö���������Speed�������ֵ
        //agent.velocity��������Դ��ı�������˼�ǻ�ȡ��ǰ���ٶȣ���һ������
        //sqrMagnitude����������ƽ������
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Death", isDead);
    }
    //�ƶ�����
    public void MoveToTarget(Vector3 target) 
    {
        StopAllCoroutines();//ֹͣЭ��,(��Ϲ���ָ��)
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;//���õ�������ķ�����ʹ�����ƶ�����λ��
    }
    //�������˵ķ���
    private void MoveToAttackTarget(GameObject target) 
    {
        if (isDead) return;
        if (target != null) //Ŀ�겻Ϊ��
        {
            attackTarget=target;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveAttack());//��ʼЭ��
        }
    }
    //�������λ�ù�����Э��
    IEnumerator MoveAttack()
    {
        agent.isStopped = false;//ȷ����������ƶ�
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);//�������
        //������ߵľ�����ڹ�������(�ݶ�Ϊ1)
        while(Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)
        {
            //ͨ��������������ƶ�������λ��
            agent.destination=attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;//�õ���ͣ����
        //����
        if (lastAttackTime < 0) 
        {
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;//���ù���cdʱ��
        }
    }
    //�������˺�
    void Hit() 
    {
        if (attackTarget.CompareTag("Attackable"))//�ǲ��ǿɹ�������Ʒ
        {
            if (attackTarget.GetComponent<Rock>()) //�ǲ���ʯͷ
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;//�л��ɹ������˵�״̬
                //��ʯͷ��ӳ����
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else 
        {
            //��ȡĿ�����ϵĽ�ɫ���Խű�
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            //��������˺��ķ���
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
