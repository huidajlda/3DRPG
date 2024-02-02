using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;//玩家预设体
    public SceneFader sceneFaderPrefab;//渐入渐出UI的预设体
    bool fabeFinished;//是否播放完毕
    GameObject player;//玩家
    NavMeshAgent playerAgent;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//过场景时不销毁此物体
    }
    void Start()
    {
        GameManager.Instance.AddObserver(this);
        fabeFinished = true;
    }
    //传送到目标的终点
    public void TransitionToDestination(TransitionPoint transitionPoint) 
    {
        switch (transitionPoint.transitionType) //判断是同场景还是异场景
        {
            case TransitionPoint.TransitionType.SameScene://同场景
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene: //异场景
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    //加载场景的协程(场景名称，目标点类型)
    IEnumerator Transition(string sceneName,TransitionDestinationPoint.DestinationTag destinationTag) 
    {
        SaveManager.Instance.SavePlayerData();//保存玩家数据
        InventoryManager.Instance.SaveData();//保存背包的数据
        QuestManager.Instance.SaveQuestSystem();//保存任务数据
        if (SceneManager.GetActiveScene().name != sceneName) //判断是不是同一场景
        {
            //FIXME:可以加入fader
            yield return SceneManager.LoadSceneAsync(sceneName);//等待场景加载完毕
            //等待玩家加载完毕
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();//加载玩家的数据
            yield break;
        }
        else //相同场景
        {
            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            //设置玩家的位置和旋转为目标点的位置和旋转
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }
    //返回目标点的位置
    private TransitionDestinationPoint GetDestination(TransitionDestinationPoint.DestinationTag destinationTag) 
    {
        var entrances = FindObjectsOfType<TransitionDestinationPoint>();//寻找所以的目标点
        for (int i = 0; i < entrances.Length; i++) 
        {
            if (entrances[i].destionationTag == destinationTag) 
            {
                return entrances[i];//返回该标记点
            }
        }
        return null;
    }
    //调用开始游戏协程的方法（给菜单提供）
    public void TransitionFirstLevel() 
    {
        StartCoroutine(LoadLevel("SampleScene"));//加载游戏场景
    }
    //开始游戏的时切换场景的协程
    IEnumerator LoadLevel(string scene) 
    {
        SceneFader fade=Instantiate(sceneFaderPrefab);//生成渐入渐出UI
        if (scene != "") //名字不为空
        {
            yield return StartCoroutine(fade.FadeOut(2.5f));//变为白屏，白屏之后加载场景和人物
            yield return SceneManager.LoadSceneAsync(scene);//加载场景
            //玩家的位置(调用GameManager里面获取玩家出生点的方法)
            //var p = GameManager.Instance.GetEntrance().position;
            //玩家的旋转
            //var r = GameManager.Instance.GetEntrance().rotation;
            Debug.Log("xxx");
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);//加载玩家
            Debug.Log("xx");
            //保存数据 
            SaveManager.Instance.SavePlayerData();
            InventoryManager.Instance.SaveData();//保存背包的数据
            yield return StartCoroutine(fade.FadeIn(2.5f));//加载完后白屏渐渐消失
            yield break;
        }
    }
    //切换到保存游戏的那个场景
    public void TransitionToLoadGame() 
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    //启动回到菜单协程的方法
    public void TransitionToMain() 
    {
        StartCoroutine(LoadMain());
    }
    //回到菜单的方法协程
    IEnumerator LoadMain() 
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//生成渐入渐出UI
        yield return StartCoroutine(fade.FadeOut(2.5f));//变为白屏，白屏之后加载场景和人物
        yield return SceneManager.LoadSceneAsync("Main");//加载菜单场景
        yield return StartCoroutine(fade.FadeIn(2.5f));//加载完后白屏渐渐消失
        yield break;
    }
    //游戏结束
    public void EndNotify()
    {
        if (fabeFinished) 
        {
            fabeFinished = false;
            StartCoroutine(LoadMain());//开启协程
        }
    }
}
