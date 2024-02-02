using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem //掉落物品类
    {
        public GameObject item;//掉落的物品
        [Range(0,1)]//将范围限制到0~1
        public float weight;//权重(概率)
    }
    public LootItem[] lootItems;//掉落物品类的数组
    //生成掉落物的方法
    public void Spawnloot() 
    {
        float currentValue=Random.value;//随机产生一个数
        for (int i = 0; i < lootItems.Length; i++) 
        {
            if (currentValue <= lootItems[i].weight) //小于权重值即掉落
            {
                GameObject obj = Instantiate(lootItems[i].item);//生成物体
                //设置一下掉落位置
                obj.transform.position = transform.position + Vector3.up * 2;
            }
        }
    }
}
