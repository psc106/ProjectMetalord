using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject questionUI;


    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueTypingEffect myTextEffect;
    private bool isSpeak = false;
    public bool isResponse = false;

    void Start()
    {
        isSpeak = false;
        myTextEffect = GetComponent<DialogueTypingEffect>();
        CloseDialogueUI();
        CloseTutoQuestion();
    }
    
    public void ShowTutoDialogue(int keyNum)
    {
        OpenDialogueUI();
        isResponse = true;
        StartCoroutine(StepThroughDialogue(keyNum));
    }


    public void ShowDialogue(int keyNum)
    {
        OpenDialogueUI();
        StartCoroutine(StepThroughDialogue(keyNum));
    }
    private IEnumerator StepThroughDialogue(int keyNum)
    {
        if (keyNum == 0)
        {
            yield break;
        }
        while (true)
        {
            string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
            string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;
            string[] contexteArray = DialogueDBManager.instance.dialogueDic[keyNum].contextes;

            for (int i = 0; i < contexteArray.Length; i++)
            {
                isSpeak = true;
                string dialogue = DialogueDBManager.instance.dialogueDic[keyNum].contextes[i];
                yield return myTextEffect.Run(dialogue, dialogueText);

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            
            if(isResponse)
            {
                //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
                OpenTutoQuestion();
                isResponse = false;
            }

            if (nextId != "")
            {
                keyNum = int.Parse(nextId);
            }
            else if (nextId == "")
            {
                isSpeak = false;
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                break;
            }
        }
        yield break;
    }


    public void OpenDialogueUI()
    {
        dialogueUI.SetActive(true);
    }
    public void CloseDialogueUI() 
    {
        dialogueUI.SetActive(false);
    }

    public void  OpenTutoQuestion()
    {
        questionUI.SetActive(true);
    }
    public void CloseTutoQuestion()
    {
        questionUI.SetActive(false);
    }

}
