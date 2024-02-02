using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    CanvasGroup CanvasGroup;//CanvasGroup组件的变量
    public float fadeInDuration;//渐入时间
    public float fadeoutDuration;//渐出时间
    void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();//获取CanvasGroup组件
        DontDestroyOnLoad(gameObject);//过场景不销毁
    }
    //调用两个协程的协程
    public IEnumerator FadeOutIn() 
    {
        yield return FadeOut(fadeoutDuration);//从没有白屏变为白屏
        yield return FadeIn(fadeInDuration); //从白屏变为没有白屏
    }
    //从没有白屏变为白屏
    public IEnumerator FadeOut(float time) 
    {
        while (CanvasGroup.alpha < 1) 
        {
            CanvasGroup.alpha+=Time.deltaTime/time;//渐变成白屏
            yield return null;
        }
    }
    //从白屏变为没有白屏
    public IEnumerator FadeIn(float time)
    {
        while (CanvasGroup.alpha !=0)
        {
            CanvasGroup.alpha -= Time.deltaTime / time;//渐变成不是白屏
            yield return null;
        }
        Destroy(gameObject);//切换完毕后删除
    }
}
