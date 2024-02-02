using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//����һ���¼����˷�����Ҫ��unity���������ק�����Ƽ�ʹ��
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }
public class oneMouseManager : Singleton<oneMouseManager>
{
    //ֱ�Ӵ����¼�����
    public event Action<Vector3> OnMouseClicked;
    //���������¼�
    public event Action<GameObject> OnEnemyClick;
    RaycastHit hitinfo;//������Ϣ
    //����һЩ���ͼƬ����
    public Texture2D point, doorway, attack, target, arrow;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        setCursorTexture();
        //����UIʱ��ִ���ƶ��ķ���
        if (InteractWithUI()) return;
        MouseControl();

    }
    //����ƶ��ķ���
    void MouseControl() 
    {
        //��������Ҽ���������ײ�����岻Ϊ��
        if (Input.GetMouseButtonDown(1)&&hitinfo.collider!=null) 
        {
            Debug.Log(hitinfo.collider.tag);
            if (hitinfo.collider.gameObject.CompareTag("Ground")) //����
            {
                OnMouseClicked?.Invoke(hitinfo.point);//�¼��Ƿ�Ϊ�գ���Ϊ���������ִ�к���
            }
            if (hitinfo.collider.gameObject.CompareTag("Enemy")) //����
            {
                OnEnemyClick?.Invoke(hitinfo.collider.gameObject);//�¼��Ƿ�Ϊ�գ���Ϊ���������ִ�к���
            }
            if (hitinfo.collider.gameObject.CompareTag("Attackable")) //�ɻ�����Ʒ
            {
                OnEnemyClick?.Invoke(hitinfo.collider.gameObject);//�¼��Ƿ�Ϊ�գ���Ϊ���������ִ�к���
            }
            if (hitinfo.collider.gameObject.CompareTag("Portal")) //������
            {
                OnMouseClicked?.Invoke(hitinfo.point);//�¼��Ƿ�Ϊ�գ���Ϊ���������ִ�к���
            }
            if (hitinfo.collider.gameObject.CompareTag("Item")) //��Ʒ
            {
                OnMouseClicked?.Invoke(hitinfo.point);//�¼��Ƿ�Ϊ�գ���Ϊ���������ִ�к���
            }

        }
    }

    //�л������ͼ����
    void setCursorTexture() 
    {
        //�����������һ�����ߣ�����ײ��λ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (InteractWithUI()) //��������UI��
        {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//�������Ϊ�֣�����ƫ��
            return;
        }
        if (Physics.Raycast(ray, out hitinfo)) 
        {
            //�л������ͼ
            switch (hitinfo.collider.gameObject.tag) 
            {
                case "Ground"://����
                    //��һ������������ͼƬ
                    //�ڶ���������ƫ��ֵ,���ֻ�����ϽǶ��˵�λ�����жϵ��
                    //���������ԲȦ��������״�ľ���Ҫ�����λ�ý���ƫ��
                    //�����õ����Ϊ32��32����ôԲ�����ƫ�ƾ���16��16
                    //CursorMode.Auto�Զ��л�
                    Cursor.SetCursor(target, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Enemy"://����
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal"://������
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item"://��Ʒ
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default://Ĭ��
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }
    //�ж�����ǲ����ڲ���UI
    bool InteractWithUI() 
    {
        if (EventSystem.current != null&&EventSystem.current.IsPointerOverGameObject()) 
        {
                return true;
        }
        else return false;
    }
}
