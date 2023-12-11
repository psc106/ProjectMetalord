using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    #region 다이얼로그 매직넘버 관련 ID
    // ShowDialogue
    // 1~10 :튜토리얼 토끼
    // 11 : 양
    // 12 : 꿀벌
    // 13 : 공룡
    // 14 : 이모키드
    // 15 : 말
    // 16 : 뿔고래
    // 17 : 공주
    #endregion
    public PlayerMove testPlayer;

    public GameObject dialogueUI;
    public GameObject questionUI;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueTypingEffect myTextEffect;
    public bool isResponse = false;

    void Start()
    {
        myTextEffect = GetComponent<DialogueTypingEffect>();
        CloseDialogueUI();
        CloseTutoQuestion();
    }
    
    public void ShowTutoDialogue(int keyNum)
    {
        Debug.LogFormat("{0}<==d이게 keyNum값",keyNum);
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
                string dialogue = DialogueDBManager.instance.dialogueDic[keyNum].contextes[i];
                dialogueBox.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    DialogueDBManager.instance.dialogueDic[keyNum].speakerName.Trim();
                yield return myTextEffect.Run(dialogue, dialogueText);

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            
            if(isResponse)
            {
                //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
                OpenTutoQuestion();
            }

            if (nextId != "")
            {
                keyNum = int.Parse(nextId);
            }
            else if (nextId == "")
            {
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                if (isResponse == false)
                {
                    CloseDialogueUI();
                    //플레이어 움직임 다시 제어
                    testPlayer.isMove = true;
                }
                break;
            }
            
            Debug.Log("이건 마지막 eKey 위에 디버그");
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

    public void ChangeResponeBoolValue(bool value)
    {
        isResponse = value;
    }
}
