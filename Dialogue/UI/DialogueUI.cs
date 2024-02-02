using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
//using DG.Tweening;
public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]//基本元素信息
    public Image icon;//头像
    public Text mainText;//主要对话文本
    public Button nextButton;//下一句话的按钮
    public GameObject dialoguePanel;//对话面板
    [Header("Option")]//选项的元素信息
    public RectTransform optionPanel;//选项画板
    public OptionUI optionPrefab;//选项按钮
    [Header("Data")]//数据信息
    public DialogueData_SO currentData;//当前需要播放的对话信息
    int currentIndex = 0;//播放对话信息中的哪一句话的索引
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);//添加点击事件
    }
    //更新对话数据
    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData=data;//当前播放的文本等于传入的文本
        currentIndex = 0;//保证每次对话从头开始
    }
    //更新对话框UI
    public void UpdataMainDialogue(DialoguePiece piece) 
    {
        dialoguePanel.SetActive(true);//激活对话框
        currentIndex++;//播放下一条对话的索引
        if (piece.image != null)
        {
            icon.enabled = true; //激活头像
            icon.sprite = piece.image;//显示头像图片
        }
        else icon.enabled = false;//不显示头像
        mainText.text = "";//清空里面以前的文本
        //mainText.text = piece.text;//将当前文本赋值进去
        mainText.DOText(piece.text, 1f);//参数：文字,事件
        if (piece.dialogueOptions.Count == 0 && currentData.dialoguePiece.Count > 0)
        {
            nextButton.interactable = true;//让按钮可以点
            //该条对话没有选项，且对话不是只有一句，才显示next按钮
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);//让文字显示
        }
        else 
        {
            //原：nextButton.gameObject.SetActive(false);//否则不显示next按钮(失活按钮)
            nextButton.interactable = false;//让按钮不可以点
            nextButton.transform.GetChild(0).gameObject.SetActive(false);//仅让文字不显示(失活文本)
        }
        //创建option对话选项
        CreateOption(piece);
    }
    //播放下一条对话
    void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePiece.Count) //索引没越界是播放下一条对话
        {
            UpdataMainDialogue(currentData.dialoguePiece[currentIndex]);
        }
        else dialoguePanel.SetActive(false);//关闭对话
    }
    //创建选项
    void CreateOption(DialoguePiece piece) 
    {
        if (optionPanel.childCount > 0) //选项画板的孩子(即选项)数量大于0
        {
            for (int i = 0; i < optionPanel.childCount; i++) 
            {
                Destroy(optionPanel.GetChild(i).gameObject);//销毁所有之前的选项
            }
        }
        //生成选项
        for (int i = 0; i < piece.dialogueOptions.Count; i++) 
        {
            var option = Instantiate(optionPrefab, optionPanel);//创建选项作为optionPanel的子物体
            option.UpdateOption(piece, piece.dialogueOptions[i]);
        }
    }
}
