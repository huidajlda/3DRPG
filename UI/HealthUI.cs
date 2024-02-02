using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Ѫ��ui�ű�
public class HealthUI : MonoBehaviour
{
    public GameObject healthUIPrefab;//Ѫ����Ԥ����
    public Transform barPoint;//Ѫ��λ��
    public bool alwaysVisible;//�Ƿ�һֱ�ɼ�
    public float visibleTime;//���ӻ�ʱ��
    public float timeleft;//ʣ����ӻ�ʱ��
    Image healthSlider;//ʣ��Ѫ��
    Transform UIbar;//��ǰλ�ã�Ѫ��ʵ����
    Transform cam;//�������λ��
    CharacterStats currentStats;
    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;//����¼�
    }
    void OnEnable()
    {
        cam = Camera.main.transform;//��ȡ�������λ��
        //��������UI���ҵ�Ѫ����UI
        foreach (Canvas canvas in FindObjectsOfType<Canvas>()) 
        {
            if (canvas.renderMode == RenderMode.WorldSpace) 
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//ʵ����Ѫ��
                healthSlider=UIbar.GetChild(0).GetComponent<Image>();//��ȡ���Ѫ����image
                UIbar.gameObject.SetActive(alwaysVisible);//Ѫ��Ĭ�ϲ���ʾ
            }
        }
    }
    //����Ѫ���ķ���
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth<=0) 
        {
            Destroy(UIbar.gameObject);//Ѫ��Ϊ0����Ѫ��
        }
        UIbar.gameObject.SetActive(true);//������ʱ��ʾѪ��
        timeleft = visibleTime;//����Ѫ�������¼�
        float sliderPercent=(float)currentHealth/maxHealth;//����ʣ��Ѫ���ٷֱ�
        healthSlider.fillAmount = sliderPercent;//������ΪѪ����image�����İٷֱ�
    }
    void LateUpdate()
    {
        if (UIbar != null) 
        {
            //Ѫ������
            UIbar.position = barPoint.position;
            UIbar.forward=-cam.forward;//��Ѫ��ʼ����������
            if (timeleft <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);//����Ѫ��
            }
            else 
            {
                timeleft-=Time.deltaTime;
            }
        }
    }
}
