using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public Text itemNameText;//��Ʒ�����ı�
    public Text itemInfoText;//��Ʒ�����ı�
    RectTransform rectTransform;//�ı�UI��λ��(RectTransform���õ���ê���λ��)
    void Awake()
    {
        rectTransform= GetComponent<RectTransform>();
    }
    //������Ʒ��Ϣ
    public void SetupToolTtip(ItemData_SO item) 
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }
    void OnEnable()
    {
        //���屻����ʱ���ȸ���һ������
        UpdatePosition();
    }
    void Update() 
    {
        UpdatePosition();
    }
    //����UI����
    public void UpdatePosition() 
    {
        Vector3 mousePos=Input.mousePosition;//��ȡ��������
        Vector3[] corners=new Vector3[4];
        //��ȡUI�ĸ��ǵ����꣬����Ϊ���£����ϣ����ϣ����£�˳ʱ�룬��������������ɻ�ÿ���
        rectTransform.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;//��ȡ��
        float height = corners[1].y - corners[0].y;//��ȡ��
        if (mousePos.y < height) //����λ��yС��UI�߶�˵������λ����ʾ
        {
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;//������Ҫ��UI������һ�����
        }
        //��Ļ��ȼ�ȥ���λ�õ�x����UI��ȣ��ſ������Ҳ���ʾ
        else if (Screen.width - mousePos.x > width)
        {
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else //С�ڰ�ui��ʾ�����
        {
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
        }
    }
}
