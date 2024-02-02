using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MainMenu : MonoBehaviour
{
    Button newGameBtn;//��ʼ����Ϸ�İ�ť
    Button continueBtn;//������Ϸ�İ�ť
    Button quitBtn;//�˳���Ϸ�İ�ť
    PlayableDirector director;//Timeline�������
    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();//��ȡ��ʼ��Ϸ�İ�ť
        continueBtn= transform.GetChild(2).GetComponent<Button>();//��ȡ������Ϸ�İ�ť
        quitBtn= transform.GetChild(3).GetComponent<Button>();//��ȡ�˳���Ϸ�İ�ť
        //���newGame���Ŷ���
        newGameBtn.onClick.AddListener(PlayTimeline);//����ʼ��Ϸ�İ�ť��ӿ�ʼ��Ϸ�ķ���
        continueBtn.onClick.AddListener(ContinueGame);//��������Ϸ�İ�ť��Ӽ�����Ϸ�ķ���
        quitBtn.onClick.AddListener(QuitGame);//���˳���Ϸ�İ�ť����˳���Ϸ�ķ���
        director = FindObjectOfType<PlayableDirector>();//�ҵ����
        //director�Դ��ģ�����������Ͼͻ�ִ�к����ķ���
        director.stopped += NewGame;//����������
    }
    void PlayTimeline() 
    {
        director.Play();//���Ŷ���
    }
    //��ʼ����Ϸ�ķ���
    void NewGame(PlayableDirector obj) 
    {
        PlayerPrefs.DeleteAll();//�������
        //ת������
        SceneController.Instance.TransitionFirstLevel();

    }
    //������Ϸ�ķ���
    void ContinueGame() 
    {
        SceneController.Instance.TransitionToLoadGame();
    }
    //�˳���Ϸ
    void QuitGame() 
    {
        Application.Quit();
        Debug.Log("�˳���Ϸ");
    }
}
