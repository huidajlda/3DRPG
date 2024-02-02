using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem //������Ʒ��
    {
        public GameObject item;//�������Ʒ
        [Range(0,1)]//����Χ���Ƶ�0~1
        public float weight;//Ȩ��(����)
    }
    public LootItem[] lootItems;//������Ʒ�������
    //���ɵ�����ķ���
    public void Spawnloot() 
    {
        float currentValue=Random.value;//�������һ����
        for (int i = 0; i < lootItems.Length; i++) 
        {
            if (currentValue <= lootItems[i].weight) //С��Ȩ��ֵ������
            {
                GameObject obj = Instantiate(lootItems[i].item);//��������
                //����һ�µ���λ��
                obj.transform.position = transform.position + Vector3.up * 2;
            }
        }
    }
}
