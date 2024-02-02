using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;//���Ԥ����
    public SceneFader sceneFaderPrefab;//���뽥��UI��Ԥ����
    bool fabeFinished;//�Ƿ񲥷����
    GameObject player;//���
    NavMeshAgent playerAgent;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//������ʱ�����ٴ�����
    }
    void Start()
    {
        GameManager.Instance.AddObserver(this);
        fabeFinished = true;
    }
    //���͵�Ŀ����յ�
    public void TransitionToDestination(TransitionPoint transitionPoint) 
    {
        switch (transitionPoint.transitionType) //�ж���ͬ���������쳡��
        {
            case TransitionPoint.TransitionType.SameScene://ͬ����
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene: //�쳡��
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    //���س�����Э��(�������ƣ�Ŀ�������)
    IEnumerator Transition(string sceneName,TransitionDestinationPoint.DestinationTag destinationTag) 
    {
        SaveManager.Instance.SavePlayerData();//�����������
        InventoryManager.Instance.SaveData();//���汳��������
        QuestManager.Instance.SaveQuestSystem();//������������
        if (SceneManager.GetActiveScene().name != sceneName) //�ж��ǲ���ͬһ����
        {
            //FIXME:���Լ���fader
            yield return SceneManager.LoadSceneAsync(sceneName);//�ȴ������������
            //�ȴ���Ҽ������
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();//������ҵ�����
            yield break;
        }
        else //��ͬ����
        {
            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            //������ҵ�λ�ú���תΪĿ����λ�ú���ת
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }
    //����Ŀ����λ��
    private TransitionDestinationPoint GetDestination(TransitionDestinationPoint.DestinationTag destinationTag) 
    {
        var entrances = FindObjectsOfType<TransitionDestinationPoint>();//Ѱ�����Ե�Ŀ���
        for (int i = 0; i < entrances.Length; i++) 
        {
            if (entrances[i].destionationTag == destinationTag) 
            {
                return entrances[i];//���ظñ�ǵ�
            }
        }
        return null;
    }
    //���ÿ�ʼ��ϷЭ�̵ķ��������˵��ṩ��
    public void TransitionFirstLevel() 
    {
        StartCoroutine(LoadLevel("SampleScene"));//������Ϸ����
    }
    //��ʼ��Ϸ��ʱ�л�������Э��
    IEnumerator LoadLevel(string scene) 
    {
        SceneFader fade=Instantiate(sceneFaderPrefab);//���ɽ��뽥��UI
        if (scene != "") //���ֲ�Ϊ��
        {
            yield return StartCoroutine(fade.FadeOut(2.5f));//��Ϊ����������֮����س���������
            yield return SceneManager.LoadSceneAsync(scene);//���س���
            //��ҵ�λ��(����GameManager�����ȡ��ҳ�����ķ���)
            //var p = GameManager.Instance.GetEntrance().position;
            //��ҵ���ת
            //var r = GameManager.Instance.GetEntrance().rotation;
            Debug.Log("xxx");
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);//�������
            Debug.Log("xx");
            //�������� 
            SaveManager.Instance.SavePlayerData();
            InventoryManager.Instance.SaveData();//���汳��������
            yield return StartCoroutine(fade.FadeIn(2.5f));//����������������ʧ
            yield break;
        }
    }
    //�л���������Ϸ���Ǹ�����
    public void TransitionToLoadGame() 
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    //�����ص��˵�Э�̵ķ���
    public void TransitionToMain() 
    {
        StartCoroutine(LoadMain());
    }
    //�ص��˵��ķ���Э��
    IEnumerator LoadMain() 
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//���ɽ��뽥��UI
        yield return StartCoroutine(fade.FadeOut(2.5f));//��Ϊ����������֮����س���������
        yield return SceneManager.LoadSceneAsync("Main");//���ز˵�����
        yield return StartCoroutine(fade.FadeIn(2.5f));//����������������ʧ
        yield break;
    }
    //��Ϸ����
    public void EndNotify()
    {
        if (fabeFinished) 
        {
            fabeFinished = false;
            StartCoroutine(LoadMain());//����Э��
        }
    }
}
