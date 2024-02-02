using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats  : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;//���������Ѫ����UI���¼�<��ǰѪ������Ѫ��>
    public CharacterData_SO templateData;//ģ������
    public CharacterData_SO characterData;//CharacterData_SO����������Ա���
    public AttackData_SO attackData;//�������Եı���
    private AttackData_SO baseAttackData;//���Եı���
    private RuntimeAnimatorController baseAnimator;//һ��ʱ�����ı���
    [Header("����")]
    public Transform weaponSlot;//���������ֵ�λ��

    [HideInInspector]//��unity����������ش�����
    public bool isCritical;//�Ƿ񱩻�
    void Awake()
    {
        if (templateData != null) 
        {
            characterData = Instantiate(templateData);
        }
        baseAttackData= Instantiate(attackData);//����һ��ʼ�Ĺ�������
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;//����һ��ʼ�Ķ���
    }
    public int MaxHealth //��Ա�������Ѫ����ֱ�ӻ�ȡcharacterData���������
    {
        //�ú����������ʱ����ʹ��
        get {if (characterData != null){return characterData.maxHealth;}else{return 0;}}
        set {characterData.maxHealth = value;}
    }
    public int CurrentHealth //��Ա���Ե�ǰѪ����ֱ�ӻ�ȡcharacterData���������
    {
        //�ú����������ʱ����ʹ��
        get { if (characterData != null) { return characterData.currentHealth; } else { return 0; } }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence //��Ա���Ի���������ֱ�ӻ�ȡcharacterData���������
    {
        //�ú����������ʱ����ʹ��
        get { if (characterData != null) { return characterData.baseDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence //��Ա���Ե�ǰ������ֱ�ӻ�ȡcharacterData���������
    {
        //�ú����������ʱ����ʹ��
        get { if (characterData != null) { return characterData.currentDefence; } else { return 0; } }
        set { characterData.currentDefence = value; }
    }
    //TODO:����
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(1000);
        }
    }
    //���˵ķ���
    public void TakeDamage(CharacterStats attacker,CharacterStats defener) 
    {
        //ʵ���˺��������ߵĹ�������ȥ�������ߵķ�����
        //Ҫô���˺���ҪôΪ0
        int damage = Mathf.Max(attacker.CurrentDamage()-defener.CurrentDefence,0);
        //Ѫ����СΪ0
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (attacker.isCritical) //�������˲����ܻ�����
        {
            defener.GetComponent<Animator>().SetTrigger("Hit"); 
        }
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//���ø���Ѫ�����¼�
        if (CurrentHealth <= 0) 
        {
            attacker.characterData.UpdateExp(characterData.killPoint);//��ȡ����ֵ
        }
         
    }
    //�����˺�������
    public void TakeDamage(int damage, CharacterStats defener) 
    {
        int currentDamage=Mathf.Max(damage-defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth-currentDamage,0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//���ø���Ѫ�����¼�
        if (CurrentHealth <= 0) 
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
        }
    }
    //����������˺�
    public int CurrentDamage() 
    {
        //�������Ե���С�˺�������˺����ֵ
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical) //���������
        {
            //�˺�*����
            coreDamage = coreDamage * attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
    //�л������ķ���
    public void ChangeWeapon(ItemData_SO weapon) 
    {
        UnEquipWeapon();//ж������
        EquipWeapon(weapon);//װ������
    }
    //TODO:װ�����Ƶķ�������
    //װ�������ķ���(��������������Ϣ)
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)//����Ԥ���岻Ϊ��
        {
            //Instantiate��һ�����أ����������ɵ�Ԥ������丸���壬�ᱣ��Ԥ����ԭ�е�position��rotation
            Instantiate(weapon.weaponPrefab, weaponSlot);//��������������
        }
        //TODO:��������
        attackData.ApplyWeaponData(weapon.weaponData);//���ø������Եķ���
        //�л�����
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }
    //ж�������ķ���
    public void UnEquipWeapon() 
    {
        if (weaponSlot.transform.childCount != 0) //����ֲ���û������
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++) 
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);//�����ֲ�������
            }
        }
        attackData.ApplyWeaponData(baseAttackData);//��ԭ����
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;//��ԭ����
    }
    #region ʹ�õ��ߺ����ݸı�ķ���
    //TODO��ʹ�õ������ݸı�ķ�����д����
    //��Ѫ�ķ���
    public void ApplyHealth(int amount) 
    {
        if (CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else //��������Ѫ
        {
            CurrentHealth = MaxHealth;
        }
    }
    #endregion
}
