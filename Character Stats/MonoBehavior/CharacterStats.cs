using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats  : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;//攻击后更新血条的UI的事件<当前血量，满血量>
    public CharacterData_SO templateData;//模板属性
    public CharacterData_SO characterData;//CharacterData_SO人物基础属性变量
    public AttackData_SO attackData;//攻击属性的变量
    private AttackData_SO baseAttackData;//属性的备份
    private RuntimeAnimatorController baseAnimator;//一开时动画的备份
    [Header("武器")]
    public Transform weaponSlot;//拿武器的手的位置

    [HideInInspector]//在unity检查器中隐藏此属性
    public bool isCritical;//是否暴击
    void Awake()
    {
        if (templateData != null) 
        {
            characterData = Instantiate(templateData);
        }
        baseAttackData= Instantiate(attackData);//保存一开始的攻击属性
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;//保存一开始的动画
    }
    public int MaxHealth //成员属性最大血量，直接获取characterData里面的数据
    {
        //让后面访问数据时方便使用
        get {if (characterData != null){return characterData.maxHealth;}else{return 0;}}
        set {characterData.maxHealth = value;}
    }
    public int CurrentHealth //成员属性当前血量，直接获取characterData里面的数据
    {
        //让后面访问数据时方便使用
        get { if (characterData != null) { return characterData.currentHealth; } else { return 0; } }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence //成员属性基础防御，直接获取characterData里面的数据
    {
        //让后面访问数据时方便使用
        get { if (characterData != null) { return characterData.baseDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence //成员属性当前防御，直接获取characterData里面的数据
    {
        //让后面访问数据时方便使用
        get { if (characterData != null) { return characterData.currentDefence; } else { return 0; } }
        set { characterData.currentDefence = value; }
    }
    //TODO:作弊
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(1000);
        }
    }
    //受伤的方法
    public void TakeDamage(CharacterStats attacker,CharacterStats defener) 
    {
        //实际伤害，攻击者的攻击力减去被攻击者的防御力
        //要么有伤害，要么为0
        int damage = Mathf.Max(attacker.CurrentDamage()-defener.CurrentDefence,0);
        //血量最小为0
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (attacker.isCritical) //被暴击了播放受击动画
        {
            defener.GetComponent<Animator>().SetTrigger("Hit"); 
        }
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//调用更新血条的事件
        if (CurrentHealth <= 0) 
        {
            attacker.characterData.UpdateExp(characterData.killPoint);//获取经验值
        }
         
    }
    //技能伤害的重载
    public void TakeDamage(int damage, CharacterStats defener) 
    {
        int currentDamage=Mathf.Max(damage-defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth-currentDamage,0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//调用更新血条的事件
        if (CurrentHealth <= 0) 
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
        }
    }
    //攻击的随机伤害
    public int CurrentDamage() 
    {
        //攻击属性的最小伤害和最大伤害间的值
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical) //如果暴击了
        {
            //伤害*爆伤
            coreDamage = coreDamage * attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
    //切换武器的方法
    public void ChangeWeapon(ItemData_SO weapon) 
    {
        UnEquipWeapon();//卸下武器
        EquipWeapon(weapon);//装备武器
    }
    //TODO:装备盾牌的方法类似
    //装备武器的方法(参数：武器的信息)
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)//武器预设体不为空
        {
            //Instantiate的一个重载，参数是生成的预设体和其父物体，会保持预设体原有的position和rotation
            Instantiate(weapon.weaponPrefab, weaponSlot);//在手上生成武器
        }
        //TODO:更新属性
        attackData.ApplyWeaponData(weapon.weaponData);//调用更新属性的方法
        //切换动画
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }
    //卸下武器的方法
    public void UnEquipWeapon() 
    {
        if (weaponSlot.transform.childCount != 0) //检查手部有没有武器
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++) 
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);//销毁手部的武器
            }
        }
        attackData.ApplyWeaponData(baseAttackData);//还原属性
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;//还原动画
    }
    #region 使用道具后数据改变的方法
    //TODO：使用道具数据改变的方法都写在这
    //回血的方法
    public void ApplyHealth(int amount) 
    {
        if (CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else //锁定在满血
        {
            CurrentHealth = MaxHealth;
        }
    }
    #endregion
}
