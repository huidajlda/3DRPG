using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,//ͬ��������
        DifferentScene,//����ͬ��������
    }
    [Header("Transition Info")]//���͵���Ϣ
    public string sceneName;//����������
    public TransitionType transitionType;//ö�ٱ���(�ж��ǲ���ͬ��������)
    public TransitionDestinationPoint.DestinationTag destinationTag;//�յ��ǩ����
    private bool canTrans;//��Ҵ���ʱ�ſ��Դ���
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans) 
        {
            Debug.Log("xx");
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    //��������
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) //����Ҳſ��Դ���
        {
            canTrans = true;
        }
    }
    //�����뿪
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) //�뿪���óɲ��ܴ���
        {
            canTrans = false;
        }
    }
}
