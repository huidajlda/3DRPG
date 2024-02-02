using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//角色控制脚本
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;//导航代理
    private Animator animator;//动画控制器
    private GameObject attackTarget;//攻击目标
    private float lastAttackTime;//攻击间隔
    private CharacterStats characterStats;//角色属性
    private bool isDead;//是否死亡
    private float stopDistance;//停止距离（攻击距离）
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//获取导航代理组件
        animator = GetComponent<Animator>();//获取动画控制器
        characterStats = GetComponent<CharacterStats>();//获取角色属性的脚本
        stopDistance = agent.stoppingDistance;
    }
    void OnEnable()//激活时添加事件注册
    {
        oneMouseManager.Instance.OnMouseClicked += MoveToTarget;//将人物移动方法添加到事件
        oneMouseManager.Instance.OnEnemyClick += MoveToAttackTarget;
        GameManager.Instance.RegisterPlayer(characterStats);//注册玩家
    }
    void Start()
    {
        SaveManager.Instance.LoadPlayerData();//加载玩家数据
    }
    void OnDisable()//销毁时取消事件订阅
    {
        Debug.Log("销毁了");
        oneMouseManager.Instance.OnMouseClicked -= MoveToTarget;
        oneMouseManager.Instance.OnEnemyClick -= MoveToAttackTarget;
    }
    void Update()
    {
        if (isDead) 
        {
            GameManager.Instance.NotifyObservers();//发送结束游戏的广播
        }
        isDead = characterStats.CurrentHealth == 0;
        SwitchAnimation();
        lastAttackTime-=Time.deltaTime;
    }
    //控制动画的切换
    private void SwitchAnimation() 
    {
        //设置动画管理器Speed里面的数值
        //agent.velocity导航组件自带的变量，意思是获取当前的速度，是一个向量
        //sqrMagnitude计算向量的平方长度
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Death", isDead);
    }
    //移动方法
    public void MoveToTarget(Vector3 target) 
    {
        StopAllCoroutines();//停止协程,(打断攻击指令)
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;//调用导航里面的方法、使人物移动到该位置
    }
    //攻击敌人的方法
    private void MoveToAttackTarget(GameObject target) 
    {
        if (isDead) return;
        if (target != null) //目标不为空
        {
            attackTarget=target;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveAttack());//开始协程
        }
    }
    //走向敌人位置攻击的协程
    IEnumerator MoveAttack()
    {
        agent.isStopped = false;//确保人物可以移动
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);//看向敌人
        //如果两者的距离大于攻击距离(暂定为1)
        while(Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)
        {
            //通过导航代理组件移动到敌人位置
            agent.destination=attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;//让导航停下来
        //攻击
        if (lastAttackTime < 0) 
        {
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;//重置攻击cd时间
        }
    }
    //玩家造成伤害
    void Hit() 
    {
        if (attackTarget.CompareTag("Attackable"))//是不是可攻击的物品
        {
            if (attackTarget.GetComponent<Rock>()) //是不是石头
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;//切换成攻击敌人的状态
                //给石头添加冲击力
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else 
        {
            //获取目标身上的角色属性脚本
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            //调用造成伤害的方法
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
