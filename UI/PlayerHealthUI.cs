using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//���UI���ƽű�
public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;//�ı�UI���ȼ���
    Image healthSlider;//ͼƬUI��Ѫ����
    Image expSlider;//������
    void Awake()
    {
        //��ȡcanves�µ������壬�����õ��ı���Ѫ���;�����
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider=transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider= transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        //�޸ĵȼ��ı���Ĭ����ʾ��λ��
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdataHealth();
        UpdataExp();
    }
    //�������Ѫ��
    void UpdataHealth() 
    {
        int currentHealth = GameManager.Instance.playerStats.CurrentHealth;//��ȡ��ҵ�ǰѪ��
        int maxhealth = GameManager.Instance.playerStats.MaxHealth;//��ȡ������Ѫ��
        float sliderPercent = (float)currentHealth / maxhealth;//����ٷֱ�
        healthSlider.fillAmount = sliderPercent;//����Ѫ��
    }
    //������Ҿ�����
    void UpdataExp() 
    {
        int currentExp = GameManager.Instance.playerStats.characterData.currentExp;//��ȡ��ҵ�ǰѪ��
        int maxExp= GameManager.Instance.playerStats.characterData.baseExp;//��ȡ������Ѫ��
        float sliderPercent = (float)currentExp / maxExp;//����ٷֱ�
        expSlider.fillAmount = sliderPercent;//���¾�����
    }
}
