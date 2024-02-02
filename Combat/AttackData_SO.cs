using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//������������
    public float skillRange;//Զ�̹�������
    public float coolDown;//CD��ȴʱ�䣨���٣�
    public float minDamage;//��С������ֵ
    public float maxDamage;//��󹥻���ֵ
    public float criticalMultiplier;//������ļӳɰٷֱȣ����ˣ�
    public float criticalChance;//�����ʣ�0~1��
    //�������Եķ���
    public void ApplyWeaponData(AttackData_SO weapon) 
    {
        //����ɫԭ���Ĺ������Զ��滻�������Ĺ�������
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        minDamage += weapon.minDamage;
        maxDamage += weapon.maxDamage;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }

}
