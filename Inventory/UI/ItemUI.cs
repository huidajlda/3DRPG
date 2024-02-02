using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;//��ʾ��ͼƬ
    public Text amount=null;//��Ʒ����
    public ItemData_SO currentItemData;//��ǰ��Ʒ������
    public InventoryData_SO Bag { get; set; }//����
    public int index { get; set; } = -1;//��Ʒ�ڱ�����������,��ʼֵΪ-1
    //���ñ���ͼƬ�������ķ���
    public void SetupItemUI(ItemData_SO item,int itemAmount) 
    {
        if (itemAmount == 0) 
        {
            //����Ϊ0ʱ�����ݴӱ������Ƴ�
            Bag.items[index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }
        //�������С��0��������������Ҫ�ύ������Ŀ������������ǲ���Ҫ���ɵ�
        if (itemAmount < 0) 
            item=null;
        if (item != null)
        {
            currentItemData = item;//���浱ǰ��Ʒ����
            icon.sprite = item.itemIcon;//����ͼƬ��ʾ
            amount.text = itemAmount.ToString();//��������
            icon.gameObject.SetActive(true);//��������
        }
        else //�����ƷΪ��,�������������
        {
            icon.gameObject.SetActive(false);//ʧ������
        }
    }
    //��ȡ��ǰUI��Ӧ��Ʒ�ķ���
    public ItemData_SO GetItem() 
    {
        return Bag.items[index].itemData;
    }
}
