using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//��������״̬��ö�ٱ�����վ׮��Ѳ�ߣ�׷����������
public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};
//���ؽű�ʱ�Զ���Ӹýű�
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;//��������״̬��ö�ٱ���
    private NavMeshAgent agent;//�������
    private Collider coll;//��ȡ��ײ��
    [Header("��������")]
    public float sightRadius;//���˿��ӷ�Χ
    public bool isGuard;//�Ƿ�Ϊվ׮״̬�ĵ���
    protected GameObject attackTarget;//���(���˵Ĺ���Ŀ��)
    private float Speed;//���˵��ٶ�
    private Animator ani;//����������
    public float lookAtTime;//Ѳ��ʱ��һ�����Ͽ���ʱ��
    private float remainLookAtTime;//Ѳ�߼�ʱ��
    private float lastAttackTime;//����cd
    [Header("Ѳ��״̬")]
    public float partrolRange;//Ѳ�߷�Χ
    private Vector3 wayPoint;//Ѳ�߷�Χ�ڵ�һ��
    private Vector3 guardPos;//һ��ʼ�����λ��
    private Quaternion guardRotation;//��¼һ��ʼ�ĽǶ�
    //�����������ϵ�bool����
    bool isWalk;//��·״̬
    bool isChase;//׷��״̬(ֻ��״̬)
    bool isFollow;//׷������
    bool isDead;//����
    bool playerDie;//�������
    protected CharacterStats characterStats;//��ɫ���ԵĿ��Ʊ���
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        coll = GetComponent<Collider>();//��ȡ��ײ��
        Speed=agent.speed;//���ڵ�����������õ��ٶ�
        guardPos = transform.position;//��ȡ��ʼ����
        guardRotation = transform.rotation;//��ȡ��ʼ�Ƕ�
        remainLookAtTime = lookAtTime;//Ѳ�߲鿴��ʱ��
        characterStats = GetComponent<CharacterStats>();
    }
    void Start()
    {
        if (isGuard) //�Ƿ�Ϊվ�ڵ���
        {
            enemyStates = EnemyStates.GUARD;//��վ׮�ĵ���
        }
        else 
        {
            enemyStates = EnemyStates.PATROL;//Ѳ�ߵĵ���
            GetNewWayPoint();//��ó�ʼ�ƶ��ĵ�
        }
        GameManager.Instance.AddObserver(this);
    }
    //�л�����ʱ������
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    void OnDisable()
    {
        if (!GameManager.IsInitialize) return;
        GameManager.Instance.RemoveObserver(this);
        if (GetComponent<LootSpawner>() && isDead) //�е�������Ľű��ҹ�������
        {
            GetComponent<LootSpawner>().Spawnloot();
        }
        if (QuestManager.IsInitialize && isDead) //����������QuestManager�Ѿ�ʵ������
        {
            //���ø���������ȵķ���
            QuestManager.Instance.UpdateQuestProgess(this.name, 1);
        }
    }
    void Update()
    {
        if (characterStats.CurrentHealth <= 0) //����
        {
            isDead = true;
        }
        if (!playerDie) 
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
        
    }
    //�л�״̬
    void SwitchStates() 
    {
        if (isDead) //�ж���û������
        {
            enemyStates=EnemyStates.DEAD;
        }
        //����������,�л���׷����״̬(CHASE)
        else if (FoundPlayer()) 
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates) 
        {
            case EnemyStates.GUARD://վ׮
                isChase = false;
                //��ǰλ�ò�����վ׮��λ��
                if (transform.position != guardPos) 
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance) 
                    {
                        isWalk = false;
                        //��ֵ��������ת��
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                        
                }
                break;
            case EnemyStates.PATROL://Ѳ��
                isChase = false;
                agent.speed = Speed*0.5f;//�ƶ��ٶ�Ϊ׷����һ��
                //�Ƿ��ߵ���Ѳ�ߵ�
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else 
                    {
                        GetNewWayPoint();
                    }
                    
                }
                else 
                {
                    isWalk=true;
                    agent.destination=wayPoint;
                }
                break;
            case EnemyStates.CHASE://׷��
                isWalk = false;//ֹͣ���ߵĶ���
                isChase = true;//��Ϊ׷���Ķ���
                ToAttackPlayer();
                break;
            case EnemyStates.DEAD://����
                coll.enabled = false;//�ر���ײ������Ҳ����ٵ��
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;
        }
    }
    //�Ƿ񿴼����
    bool FoundPlayer() 
    {
        //�ҵ���ҵ���ײ��
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var collider in colliders) 
        {
            if (collider.CompareTag("Player")) 
            {
                attackTarget=collider.gameObject;//�������
                return true;
            }
        }
        attackTarget = null;//��ʧ���
        return false;
    }
    //����׷����ҵķ���
    void ToAttackPlayer() 
    {
        agent.speed = Speed;
        if (!FoundPlayer()) //û�з������(����)
        {
            //�ص���һ��״̬(վ׮����Ѳ��)
            isFollow = false;//ֹͣ����
            if (remainLookAtTime > 0)//�ȿ�һ�£��ڷ���
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;//�ص�վ��
            }
            else 
            {
                enemyStates = EnemyStates.PATROL;//�ص�Ѳ��
            }
        }
        else 
        {
            isFollow=true;//����׷������״̬
            agent.isStopped = false;//�ڹ�����������ֹͣ���ƶ�����������뿪��Χ��Ҫ����
            agent.destination=attackTarget.transform.position;
        }
        //�ڽ�ս��Զ�̹�����Χ��
        if (TargetInAttackRange() || TargetInSkillRange()) 
        {
            isFollow = false;//���ڸ��������
            agent.isStopped = true;//���ƶ�
            if (lastAttackTime < 0) //����cdΪ0
            {
                lastAttackTime = characterStats.attackData.coolDown;
                //�����ж�
                //Random.value����0~1ֱ�ӵ�һ�������
                //���ص������С�ڱ����ʵ�ֵ��Ϊ����
                characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                //ִ�й���
                Attack();//���ù�������
            }
        }

    }
    //�л�����״̬�ĺ���
    void SwitchAnimation() 
    {
        ani.SetBool("Walk", isWalk);
        ani.SetBool("Chase", isChase);
        ani.SetBool("Follow", isFollow);
        ani.SetBool("Critical", characterStats.isCritical);
        ani.SetBool("Death", isDead);
    }
    //��ȡѲ�߷�Χ�ڵ�һ��
    void GetNewWayPoint() 
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-partrolRange, partrolRange);//��÷�Χ�ڵ����x
        float randomZ = Random.Range(-partrolRange, partrolRange);//��÷�Χ�ڵ����z
        Vector3 randomPoint=new Vector3(guardPos.x+randomX,transform.position.y,guardPos.z+randomZ);
        NavMeshHit hit;
        //�жϸõ��ڲ��ڵ��������ϣ�����bool
        //hit:�����ǰ����������ص���Ϣ���жϵķ�Χ��������ײ�ĵ�����������
        //�Ƿ��أ����Ǳ���ԭ����λ��
        wayPoint=NavMesh.SamplePosition(randomPoint, out hit, partrolRange, 1)?hit.position:transform.position;
    }
    //��ս�����ķ�Χ����
    bool TargetInAttackRange() 
    {
        if (attackTarget != null)
        {
            //��Һ͵��˵ľ���С�ڵ��˵Ĺ�������
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else 
        {
            return false;
        }
    }
    //��Զ�̹����ķ�Χ����
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            //��Һ͵��˵ľ���С�ڵ��˵Ĺ�������
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }
    //��������
    void Attack() 
    {
        transform.LookAt(attackTarget.transform);//�������
        if (TargetInAttackRange()) 
        {
            //����������
            ani.SetTrigger("Attack");
        }
        if (TargetInSkillRange()) 
        {
            //���ܹ�������(Զ��)
            ani.SetTrigger("Skill");
        }
    }
    //��������˺��ķ���
    void Hit() 
    {
        if (attackTarget != null&&transform.IsFacingTarget(attackTarget.transform)) //Ŀ�겻Ϊ�ղ�����˺�
        {
            //��ȡĿ�����ϵĽ�ɫ���Խű�
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Debug.Log("����˺�");
            //��������˺��ķ���
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
    //�ڳ������ڻ�����Χ�ķ���(ѡ���������ַ�Χ)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;//���ߵ���ɫ
        //��һ��Բ(���ĵ㣬�뾶),�������˵���Ұ��Χ
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
    //������Ϸ
    public void EndNotify()
    {
        //��ʤ����
        //ֹͣ�����ƶ�
        //ֹͣAgent
        ani.SetBool("Win", true);
        playerDie = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
