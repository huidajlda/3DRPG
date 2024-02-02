using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//创建敌人状态的枚举变量（站桩，巡逻，追击，死亡）
public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};
//挂载脚本时自动添加该脚本
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;//声明敌人状态的枚举变量
    private NavMeshAgent agent;//导航组件
    private Collider coll;//获取碰撞体
    [Header("基础设置")]
    public float sightRadius;//敌人可视范围
    public bool isGuard;//是否为站桩状态的敌人
    protected GameObject attackTarget;//玩家(敌人的攻击目标)
    private float Speed;//敌人的速度
    private Animator ani;//动画播放器
    public float lookAtTime;//巡逻时在一个点上看的时间
    private float remainLookAtTime;//巡逻计时器
    private float lastAttackTime;//攻击cd
    [Header("巡逻状态")]
    public float partrolRange;//巡逻范围
    private Vector3 wayPoint;//巡逻范围内的一点
    private Vector3 guardPos;//一开始坐标的位置
    private Quaternion guardRotation;//记录一开始的角度
    //动画播放器上的bool参数
    bool isWalk;//走路状态
    bool isChase;//追击状态(只是状态)
    bool isFollow;//追击跟随
    bool isDead;//死亡
    bool playerDie;//玩家死亡
    protected CharacterStats characterStats;//角色属性的控制变量
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        coll = GetComponent<Collider>();//获取碰撞体
        Speed=agent.speed;//等于导航面板上设置的速度
        guardPos = transform.position;//获取初始坐标
        guardRotation = transform.rotation;//获取初始角度
        remainLookAtTime = lookAtTime;//巡逻查看的时间
        characterStats = GetComponent<CharacterStats>();
    }
    void Start()
    {
        if (isGuard) //是否为站岗敌人
        {
            enemyStates = EnemyStates.GUARD;//是站桩的敌人
        }
        else 
        {
            enemyStates = EnemyStates.PATROL;//巡逻的敌人
            GetNewWayPoint();//获得初始移动的点
        }
        GameManager.Instance.AddObserver(this);
    }
    //切换场景时再启用
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    void OnDisable()
    {
        if (!GameManager.IsInitialize) return;
        GameManager.Instance.RemoveObserver(this);
        if (GetComponent<LootSpawner>() && isDead) //有掉落物体的脚本且怪物死亡
        {
            GetComponent<LootSpawner>().Spawnloot();
        }
        if (QuestManager.IsInitialize && isDead) //敌人死亡且QuestManager已经实例化了
        {
            //调用更新任务进度的方法
            QuestManager.Instance.UpdateQuestProgess(this.name, 1);
        }
    }
    void Update()
    {
        if (characterStats.CurrentHealth <= 0) //死亡
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
    //切换状态
    void SwitchStates() 
    {
        if (isDead) //判断有没有死亡
        {
            enemyStates=EnemyStates.DEAD;
        }
        //如果发现玩家,切换到追击的状态(CHASE)
        else if (FoundPlayer()) 
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates) 
        {
            case EnemyStates.GUARD://站桩
                isChase = false;
                //当前位置不等于站桩的位置
                if (transform.position != guardPos) 
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance) 
                    {
                        isWalk = false;
                        //插值（缓慢旋转）
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                        
                }
                break;
            case EnemyStates.PATROL://巡逻
                isChase = false;
                agent.speed = Speed*0.5f;//移动速度为追击的一半
                //是否走到了巡逻点
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
            case EnemyStates.CHASE://追击
                isWalk = false;//停止行走的动画
                isChase = true;//改为追击的动画
                ToAttackPlayer();
                break;
            case EnemyStates.DEAD://死亡
                coll.enabled = false;//关闭碰撞，让玩家不能再点击
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;
        }
    }
    //是否看见玩家
    bool FoundPlayer() 
    {
        //找到玩家的碰撞体
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var collider in colliders) 
        {
            if (collider.CompareTag("Player")) 
            {
                attackTarget=collider.gameObject;//发现玩家
                return true;
            }
        }
        attackTarget = null;//丢失玩家
        return false;
    }
    //敌人追击玩家的方法
    void ToAttackPlayer() 
    {
        agent.speed = Speed;
        if (!FoundPlayer()) //没有发现玩家(拉脱)
        {
            //回到上一个状态(站桩或者巡逻)
            isFollow = false;//停止跟随
            if (remainLookAtTime > 0)//先看一下，在返回
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;//回到站岗
            }
            else 
            {
                enemyStates = EnemyStates.PATROL;//回到巡逻
            }
        }
        else 
        {
            isFollow=true;//进入追击跟随状态
            agent.isStopped = false;//在攻击函数里面停止了移动，所以玩家离开范围后要开启
            agent.destination=attackTarget.transform.position;
        }
        //在近战或远程攻击范围内
        if (TargetInAttackRange() || TargetInSkillRange()) 
        {
            isFollow = false;//不在跟随玩家跑
            agent.isStopped = true;//不移动
            if (lastAttackTime < 0) //攻击cd为0
            {
                lastAttackTime = characterStats.attackData.coolDown;
                //暴击判断
                //Random.value返回0~1直接的一个随机数
                //返回的随机数小于暴击率的值视为暴击
                characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                //执行攻击
                Attack();//调用攻击函数
            }
        }

    }
    //切换动画状态的函数
    void SwitchAnimation() 
    {
        ani.SetBool("Walk", isWalk);
        ani.SetBool("Chase", isChase);
        ani.SetBool("Follow", isFollow);
        ani.SetBool("Critical", characterStats.isCritical);
        ani.SetBool("Death", isDead);
    }
    //获取巡逻范围内的一点
    void GetNewWayPoint() 
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-partrolRange, partrolRange);//获得范围内的随机x
        float randomZ = Random.Range(-partrolRange, partrolRange);//获得范围内的随机z
        Vector3 randomPoint=new Vector3(guardPos.x+randomX,transform.position.y,guardPos.z+randomZ);
        NavMeshHit hit;
        //判断该点在不在导航网格上，返回bool
        //hit:输出当前点上坐标相关的信息，判断的范围，可以碰撞的导航网格区域
        //是返回，不是保持原来的位置
        wayPoint=NavMesh.SamplePosition(randomPoint, out hit, partrolRange, 1)?hit.position:transform.position;
    }
    //近战攻击的范围里面
    bool TargetInAttackRange() 
    {
        if (attackTarget != null)
        {
            //玩家和敌人的距离小于敌人的攻击距离
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else 
        {
            return false;
        }
    }
    //在远程攻击的范围里面
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            //玩家和敌人的距离小于敌人的攻击距离
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }
    //攻击函数
    void Attack() 
    {
        transform.LookAt(attackTarget.transform);//看向玩家
        if (TargetInAttackRange()) 
        {
            //近身攻击动画
            ani.SetTrigger("Attack");
        }
        if (TargetInSkillRange()) 
        {
            //技能攻击动画(远程)
            ani.SetTrigger("Skill");
        }
    }
    //敌人造成伤害的方法
    void Hit() 
    {
        if (attackTarget != null&&transform.IsFacingTarget(attackTarget.transform)) //目标不为空才造成伤害
        {
            //获取目标身上的角色属性脚本
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Debug.Log("造成伤害");
            //调用造成伤害的方法
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
    //在场景窗口画出范围的方法(选中物体会出现范围)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;//画线的颜色
        //画一个圆(中心点，半径),画出敌人的视野范围
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
    //结束游戏
    public void EndNotify()
    {
        //获胜动画
        //停止所以移动
        //停止Agent
        ani.SetBool("Win", true);
        playerDie = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
