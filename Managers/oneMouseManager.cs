using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//定义一个事件，此方法需要在unity检查器中拖拽，不推荐使用
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }
public class oneMouseManager : Singleton<oneMouseManager>
{
    //直接创建事件变量
    public event Action<Vector3> OnMouseClicked;
    //创建攻击事件
    public event Action<GameObject> OnEnemyClick;
    RaycastHit hitinfo;//射线信息
    //创建一些鼠标图片变量
    public Texture2D point, doorway, attack, target, arrow;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        setCursorTexture();
        //操做UI时不执行移动的方法
        if (InteractWithUI()) return;
        MouseControl();

    }
    //鼠标移动的方法
    void MouseControl() 
    {
        //按下鼠标右键且射线碰撞的物体不为空
        if (Input.GetMouseButtonDown(1)&&hitinfo.collider!=null) 
        {
            Debug.Log(hitinfo.collider.tag);
            if (hitinfo.collider.gameObject.CompareTag("Ground")) //地面
            {
                OnMouseClicked?.Invoke(hitinfo.point);//事件是否为空，不为空则传入参数执行函数
            }
            if (hitinfo.collider.gameObject.CompareTag("Enemy")) //敌人
            {
                OnEnemyClick?.Invoke(hitinfo.collider.gameObject);//事件是否为空，不为空则传入参数执行函数
            }
            if (hitinfo.collider.gameObject.CompareTag("Attackable")) //可击打物品
            {
                OnEnemyClick?.Invoke(hitinfo.collider.gameObject);//事件是否为空，不为空则传入参数执行函数
            }
            if (hitinfo.collider.gameObject.CompareTag("Portal")) //传送门
            {
                OnMouseClicked?.Invoke(hitinfo.point);//事件是否为空，不为空则传入参数执行函数
            }
            if (hitinfo.collider.gameObject.CompareTag("Item")) //物品
            {
                OnMouseClicked?.Invoke(hitinfo.point);//事件是否为空，不为空则传入参数执行函数
            }

        }
    }

    //切换鼠标贴图函数
    void setCursorTexture() 
    {
        //从摄像机发出一条射线，到碰撞的位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (InteractWithUI()) //如果鼠标在UI上
        {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//设置鼠标为手，不做偏移
            return;
        }
        if (Physics.Raycast(ray, out hitinfo)) 
        {
            //切换鼠标贴图
            switch (hitinfo.collider.gameObject.tag) 
            {
                case "Ground"://地面
                    //第一个参数是鼠标的图片
                    //第二个参数是偏移值,鼠标只有左上角顶端的位置能判断点击
                    //所以如果有圆圈或其他形状的就需要将鼠标位置进行偏移
                    //这里用的鼠标为32，32，那么圆形鼠标偏移就是16，16
                    //CursorMode.Auto自动切换
                    Cursor.SetCursor(target, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Enemy"://敌人
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal"://传送门
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item"://物品
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default://默认
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }
    //判断鼠标是不是在操作UI
    bool InteractWithUI() 
    {
        if (EventSystem.current != null&&EventSystem.current.IsPointerOverGameObject()) 
        {
                return true;
        }
        else return false;
    }
}
