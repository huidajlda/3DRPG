using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//血条ui脚本
public class HealthUI : MonoBehaviour
{
    public GameObject healthUIPrefab;//血条的预制体
    public Transform barPoint;//血条位置
    public bool alwaysVisible;//是否一直可见
    public float visibleTime;//可视化时间
    public float timeleft;//剩余可视化时间
    Image healthSlider;//剩余血量
    Transform UIbar;//当前位置（血条实例）
    Transform cam;//摄像机的位置
    CharacterStats currentStats;
    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;//添加事件
    }
    void OnEnable()
    {
        cam = Camera.main.transform;//获取摄像机的位置
        //遍历所有UI，找到血条的UI
        foreach (Canvas canvas in FindObjectsOfType<Canvas>()) 
        {
            if (canvas.renderMode == RenderMode.WorldSpace) 
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//实例化血条
                healthSlider=UIbar.GetChild(0).GetComponent<Image>();//获取填充血条的image
                UIbar.gameObject.SetActive(alwaysVisible);//血条默认不显示
            }
        }
    }
    //更新血条的方法
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth<=0) 
        {
            Destroy(UIbar.gameObject);//血量为0销毁血条
        }
        UIbar.gameObject.SetActive(true);//被攻击时显示血条
        timeleft = visibleTime;//设置血条可视事件
        float sliderPercent=(float)currentHealth/maxHealth;//计算剩余血条百分比
        healthSlider.fillAmount = sliderPercent;//设置作为血条的image的填充的百分比
    }
    void LateUpdate()
    {
        if (UIbar != null) 
        {
            //血条跟随
            UIbar.position = barPoint.position;
            UIbar.forward=-cam.forward;//让血条始终面对摄像机
            if (timeleft <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);//隐藏血条
            }
            else 
            {
                timeleft-=Time.deltaTime;
            }
        }
    }
}
