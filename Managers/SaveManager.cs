using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager :Singleton<SaveManager>
{
    string sceneName="level";//��������
    //�ⲿ��ȡ�������Ƶ�����
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//��������ɾ��
    }
    void Update()
    {
        //����ʹ��
        if (Input.GetKeyDown(KeyCode.J)) //����J��������
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.K))//���¼�������
        {
            LoadPlayerData();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //����Esc�ص��˵�
        {
            SceneController.Instance.TransitionToMain();
        }
    }
    public void SavePlayerData() //�����������
    {
        //(������ݣ������ļ�������)
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    public void LoadPlayerData() //�����������
    {
        //(������ݣ������ļ�������)
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    //�������ݷ���
    public void Save(Object data,string key)
    {
        //�ڶ�������true������д���ļ�ʱ���ÿ�Щ
        var jsonData=JsonUtility.ToJson(data,true);//��Ҫ���������ת��Ϊjson��ʽ
        PlayerPrefs.SetString(key, jsonData);//��������
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);//���浱ǰ����������
        PlayerPrefs.Save();
            
    }
    //�������ݷ���
    public void Load(Object data, string key) 
    {
        if (PlayerPrefs.HasKey(key)) //�ж�PlayerPrefs����û����Ӧ��ֵ�ļ�
        {
            //��json��ʽ������ת���ɶ�Ӧ������
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);//��PlayerPrefs��ֵд�ص�data����
        }
    }
}
