using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    CanvasGroup CanvasGroup;//CanvasGroup����ı���
    public float fadeInDuration;//����ʱ��
    public float fadeoutDuration;//����ʱ��
    void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();//��ȡCanvasGroup���
        DontDestroyOnLoad(gameObject);//������������
    }
    //��������Э�̵�Э��
    public IEnumerator FadeOutIn() 
    {
        yield return FadeOut(fadeoutDuration);//��û�а�����Ϊ����
        yield return FadeIn(fadeInDuration); //�Ӱ�����Ϊû�а���
    }
    //��û�а�����Ϊ����
    public IEnumerator FadeOut(float time) 
    {
        while (CanvasGroup.alpha < 1) 
        {
            CanvasGroup.alpha+=Time.deltaTime/time;//����ɰ���
            yield return null;
        }
    }
    //�Ӱ�����Ϊû�а���
    public IEnumerator FadeIn(float time)
    {
        while (CanvasGroup.alpha !=0)
        {
            CanvasGroup.alpha -= Time.deltaTime / time;//����ɲ��ǰ���
            yield return null;
        }
        Destroy(gameObject);//�л���Ϻ�ɾ��
    }
}
