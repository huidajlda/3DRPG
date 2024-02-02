using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MainMenu : MonoBehaviour
{
    Button newGameBtn;//开始新游戏的按钮
    Button continueBtn;//继续游戏的按钮
    Button quitBtn;//退出游戏的按钮
    PlayableDirector director;//Timeline控制组件
    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();//获取开始游戏的按钮
        continueBtn= transform.GetChild(2).GetComponent<Button>();//获取继续游戏的按钮
        quitBtn= transform.GetChild(3).GetComponent<Button>();//获取退出游戏的按钮
        //点击newGame播放动画
        newGameBtn.onClick.AddListener(PlayTimeline);//给开始游戏的按钮添加开始游戏的方法
        continueBtn.onClick.AddListener(ContinueGame);//给继续游戏的按钮添加继续游戏的方法
        quitBtn.onClick.AddListener(QuitGame);//给退出游戏的按钮添加退出游戏的方法
        director = FindObjectOfType<PlayableDirector>();//找到组件
        //director自带的，动画播放完毕就会执行函数的方法
        director.stopped += NewGame;//添加世界调用
    }
    void PlayTimeline() 
    {
        director.Play();//播放动画
    }
    //开始新游戏的方法
    void NewGame(PlayableDirector obj) 
    {
        PlayerPrefs.DeleteAll();//清空数据
        //转换场景
        SceneController.Instance.TransitionFirstLevel();

    }
    //继续游戏的方法
    void ContinueGame() 
    {
        SceneController.Instance.TransitionToLoadGame();
    }
    //退出游戏
    void QuitGame() 
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
