using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//������unity�����Ҽ�����Data�����ļ�
[CreateAssetMenu(fileName ="newData",menuName ="Character Stats/Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]//������Ϣ
    public int maxHealth;//���Ѫ��
    public int currentHealth;//��ǰѪ��
    public int baseDefence;//��������
    public int currentDefence;//��ǰ����
    [Header("Kill")]
    public int killPoint;//��ɱ��������õľ���ֵ
    [Header("Level")]//�������
    public int currentLevel;//��ǰ�ȼ�
    public int maxLevel;//��ߵȼ�
    public int baseExp;//�����Ļ�������ֵ(ÿ�����)
    public int currentExp;//��ǰ����ֵ
    public float levelBuff;//���������İٷֱ�
    public float LevelMultiplier //��ÿ�����������Բ�һ��
    {
        get { return 1+(currentLevel-1)*levelBuff; }
    }
    //��ȡ����ֵ�ķ�����������ɱ�ֵľ���ֵ��
    public void UpdateExp(int point) 
    {
        currentExp += point;//���ӵ�ǰ�ľ���ֵ
        if (currentExp >= baseExp) //��ǰ����ֵ���ڻ�������ֵ��������
        {
            LevelUp();
        }
    }
    //�����ķ���
    private void LevelUp()
    {
        //����Ҫ���������ݶ�д����
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);//�ȼ�+1�����ᳬ�����ȼ�
        baseExp += (int)(baseExp * LevelMultiplier);//��һ������Ҫ�ľ���
        maxHealth = (int)(maxHealth * LevelMultiplier);//�������Ѫ��
        currentHealth = maxHealth;//������ָ���Ѫ
        baseDefence = (int)(baseDefence * LevelMultiplier);//������ķ���
        currentDefence = baseDefence;
        Debug.Log("Level UP!" + currentHealth + "Max Health" + maxHealth);
    }


}
