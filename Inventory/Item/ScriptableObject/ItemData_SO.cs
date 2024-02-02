using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType //��Ʒ����
{
    Useable,//��ʹ�õģ�����Ѫƿ
    Weapon,//��������
    Armor//װ��
}
//�����ڲ˵��ϴ�����Ʒ����
[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;//��Ʒ����
    public string itemName;//��Ʒ����
    public Sprite itemIcon;//��Ʒ�ڱ����е�ͼƬ
    public int itemAmount;//����
    public bool stackable;//�Ƿ���Զѵ�
    [Header("������Ϣ")]
    public GameObject weaponPrefab;//������Ԥ����
    public AttackData_SO weaponData;//�����Ĺ�������
    [Header("��ʹ����Ʒ����������")]
    public UseableItemData_SO useableData;
    [Header("��������")]
    //�����Ķ���
    public AnimatorOverrideController weaponAnimator;
    [TextArea]//���ַ������������ı���
    public string description="";//��Ʒ����,Ĭ��Ϊ��
}
