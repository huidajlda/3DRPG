using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager :Singleton<SaveManager>
{
    string sceneName="level";//场景名称
    //外部获取场景名称的属性
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//过场景不删除
    }
    void Update()
    {
        //测试使用
        if (Input.GetKeyDown(KeyCode.J)) //按下J保存数据
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.K))//按下加载数据
        {
            LoadPlayerData();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //按下Esc回到菜单
        {
            SceneController.Instance.TransitionToMain();
        }
    }
    public void SavePlayerData() //保存玩家数据
    {
        //(玩家数据，数据文件的名字)
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    public void LoadPlayerData() //加载玩家数据
    {
        //(玩家数据，数据文件的名字)
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    //保存数据方法
    public void Save(Object data,string key)
    {
        //第二个参数true但是是写入文件时更好看些
        var jsonData=JsonUtility.ToJson(data,true);//将要保存的数据转换为json格式
        PlayerPrefs.SetString(key, jsonData);//保存数据
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);//保存当前场景的名字
        PlayerPrefs.Save();
            
    }
    //加载数据方法
    public void Load(Object data, string key) 
    {
        if (PlayerPrefs.HasKey(key)) //判断PlayerPrefs上有没有相应的值的键
        {
            //将json格式的数据转换成对应的数据
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);//将PlayerPrefs的值写回到data里面
        }
    }
}
