using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public Text itemNameText;//物品名称文本
    public Text itemInfoText;//物品描述文本
    RectTransform rectTransform;//文本UI的位置(RectTransform设置的是锚点的位置)
    void Awake()
    {
        rectTransform= GetComponent<RectTransform>();
    }
    //设置物品信息
    public void SetupToolTtip(ItemData_SO item) 
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }
    void OnEnable()
    {
        //物体被启用时就先更新一下坐标
        UpdatePosition();
    }
    void Update() 
    {
        UpdatePosition();
    }
    //更新UI坐标
    public void UpdatePosition() 
    {
        Vector3 mousePos=Input.mousePosition;//获取鼠标的坐标
        Vector3[] corners=new Vector3[4];
        //获取UI四个角的坐标，依次为左下，左上，右上，右下，顺时针，其中两点相减即可获得宽或高
        rectTransform.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;//获取宽
        float height = corners[1].y - corners[0].y;//获取高
        if (mousePos.y < height) //鼠标的位置y小于UI高度说明不够位置显示
        {
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;//所以需要吧UI往上提一点距离
        }
        //屏幕宽度减去鼠标位置的x大于UI宽度，才可以在右侧显示
        else if (Screen.width - mousePos.x > width)
        {
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else //小于把ui显示在左侧
        {
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
        }
    }
}
