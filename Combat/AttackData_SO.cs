using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//基本攻击距离
    public float skillRange;//远程攻击距离
    public float coolDown;//CD冷却时间（攻速）
    public float minDamage;//最小攻击数值
    public float maxDamage;//最大攻击数值
    public float criticalMultiplier;//暴击后的加成百分比（爆伤）
    public float criticalChance;//暴击率（0~1）
    //更新属性的方法
    public void ApplyWeaponData(AttackData_SO weapon) 
    {
        //将角色原本的攻击属性都替换成武器的攻击属性
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        minDamage += weapon.minDamage;
        maxDamage += weapon.maxDamage;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }

}
