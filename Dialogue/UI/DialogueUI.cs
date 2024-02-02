using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
//using DG.Tweening;
public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]//����Ԫ����Ϣ
    public Image icon;//ͷ��
    public Text mainText;//��Ҫ�Ի��ı�
    public Button nextButton;//��һ�仰�İ�ť
    public GameObject dialoguePanel;//�Ի����
    [Header("Option")]//ѡ���Ԫ����Ϣ
    public RectTransform optionPanel;//ѡ���
    public OptionUI optionPrefab;//ѡ�ť
    [Header("Data")]//������Ϣ
    public DialogueData_SO currentData;//��ǰ��Ҫ���ŵĶԻ���Ϣ
    int currentIndex = 0;//���ŶԻ���Ϣ�е���һ�仰������
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);//��ӵ���¼�
    }
    //���¶Ի�����
    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData=data;//��ǰ���ŵ��ı����ڴ�����ı�
        currentIndex = 0;//��֤ÿ�ζԻ���ͷ��ʼ
    }
    //���¶Ի���UI
    public void UpdataMainDialogue(DialoguePiece piece) 
    {
        dialoguePanel.SetActive(true);//����Ի���
        currentIndex++;//������һ���Ի�������
        if (piece.image != null)
        {
            icon.enabled = true; //����ͷ��
            icon.sprite = piece.image;//��ʾͷ��ͼƬ
        }
        else icon.enabled = false;//����ʾͷ��
        mainText.text = "";//���������ǰ���ı�
        //mainText.text = piece.text;//����ǰ�ı���ֵ��ȥ
        mainText.DOText(piece.text, 1f);//����������,�¼�
        if (piece.dialogueOptions.Count == 0 && currentData.dialoguePiece.Count > 0)
        {
            nextButton.interactable = true;//�ð�ť���Ե�
            //�����Ի�û��ѡ��ҶԻ�����ֻ��һ�䣬����ʾnext��ť
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);//��������ʾ
        }
        else 
        {
            //ԭ��nextButton.gameObject.SetActive(false);//������ʾnext��ť(ʧ�ť)
            nextButton.interactable = false;//�ð�ť�����Ե�
            nextButton.transform.GetChild(0).gameObject.SetActive(false);//�������ֲ���ʾ(ʧ���ı�)
        }
        //����option�Ի�ѡ��
        CreateOption(piece);
    }
    //������һ���Ի�
    void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePiece.Count) //����ûԽ���ǲ�����һ���Ի�
        {
            UpdataMainDialogue(currentData.dialoguePiece[currentIndex]);
        }
        else dialoguePanel.SetActive(false);//�رնԻ�
    }
    //����ѡ��
    void CreateOption(DialoguePiece piece) 
    {
        if (optionPanel.childCount > 0) //ѡ���ĺ���(��ѡ��)��������0
        {
            for (int i = 0; i < optionPanel.childCount; i++) 
            {
                Destroy(optionPanel.GetChild(i).gameObject);//��������֮ǰ��ѡ��
            }
        }
        //����ѡ��
        for (int i = 0; i < piece.dialogueOptions.Count; i++) 
        {
            var option = Instantiate(optionPrefab, optionPanel);//����ѡ����ΪoptionPanel��������
            option.UpdateOption(piece, piece.dialogueOptions[i]);
        }
    }
}
