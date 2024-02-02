using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//玩家UI控制脚本
public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;//文本UI（等级）
    Image healthSlider;//图片UI（血条）
    Image expSlider;//经验条
    void Awake()
    {
        //获取canves下的子物体，创建好的文本，血条和经验条
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider=transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider= transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        //修改等级文本，默认显示两位数
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdataHealth();
        UpdataExp();
    }
    //更新玩家血条
    void UpdataHealth() 
    {
        int currentHealth = GameManager.Instance.playerStats.CurrentHealth;//获取玩家当前血量
        int maxhealth = GameManager.Instance.playerStats.MaxHealth;//获取玩家最大血量
        float sliderPercent = (float)currentHealth / maxhealth;//计算百分比
        healthSlider.fillAmount = sliderPercent;//更新血条
    }
    //更新玩家经验条
    void UpdataExp() 
    {
        int currentExp = GameManager.Instance.playerStats.characterData.currentExp;//获取玩家当前血量
        int maxExp= GameManager.Instance.playerStats.characterData.baseExp;//获取玩家最大血量
        float sliderPercent = (float)currentExp / maxExp;//计算百分比
        expSlider.fillAmount = sliderPercent;//更新经验条
    }
}
