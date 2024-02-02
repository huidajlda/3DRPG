using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//可以让unity里面右键场景Data数据文件
[CreateAssetMenu(fileName ="newData",menuName ="Character Stats/Data")] 
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]//属性信息
    public int maxHealth;//最大血量
    public int currentHealth;//当前血量
    public int baseDefence;//基础防御
    public int currentDefence;//当前防御
    [Header("Kill")]
    public int killPoint;//击杀怪物所获得的经验值
    [Header("Level")]//升级相关
    public int currentLevel;//当前等级
    public int maxLevel;//最高等级
    public int baseExp;//升级的基础经验值(每级提高)
    public int currentExp;//当前经验值
    public float levelBuff;//属性提升的百分比
    public float LevelMultiplier //让每级提升的属性不一样
    {
        get { return 1+(currentLevel-1)*levelBuff; }
    }
    //获取经验值的方法（参数击杀怪的经验值）
    public void UpdateExp(int point) 
    {
        currentExp += point;//增加当前的经验值
        if (currentExp >= baseExp) //当前经验值大于基础经验值即可升级
        {
            LevelUp();
        }
    }
    //升级的方法
    private void LevelUp()
    {
        //所有要提升的数据都写在这
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);//等级+1但不会超出最大等级
        baseExp += (int)(baseExp * LevelMultiplier);//下一级所需要的经验
        maxHealth = (int)(maxHealth * LevelMultiplier);//提升后的血量
        currentHealth = maxHealth;//升级后恢复满血
        baseDefence = (int)(baseDefence * LevelMultiplier);//提升后的防御
        currentDefence = baseDefence;
        Debug.Log("Level UP!" + currentHealth + "Max Health" + maxHealth);
    }


}
