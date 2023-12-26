using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    #region 다이얼로그 매직넘버 관련 ID
    // TODO 수정 예정
    //ShowDialogue
    // 01 ~ 12 : 튜토리얼 토끼
    // 13 ~ 15 : 양
    // 16 ~ 18 : 꿀벌
    // 19 ~ 21 : 공룡
    // 22 ~ 24 : 은둔형
    // 25 ~ 27 : 말
    // 28 ~ 30 : 뿔고래
    // 31 ~ 33 : 공주
    // 34 ~ 36 : 모험가
    #endregion

    public PlayerInteractNpc playerInteract;

    public GameObject dialogueUI;
    public GameObject questionUI;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [Header("TextFontSize")]
    public float minFontSize = 10f;
    public float maxFontSize = 50f;

    private DialogueTypingEffect myTextEffect;
    [Header("Response")]
    public bool isResponse = false;

    [Header("TextSound")]
    //12.15 사운드 작업을 위한 추가
    private AudioSource myAduio = default;
    private AudioClip textSound = default;

    

    void Start()
    {
        myAduio = GetComponent<AudioSource>();
        myAduio.clip = textSound;
        myTextEffect = GetComponent<DialogueTypingEffect>();
        CloseDialogueUI();
        CloseTutoQuestion();
       // ChangeFontSize(6);
    }
    #region TODO 삭제예정
    //public void ShowTutoDialogue(int keyNum)
    //{
    //    OpenDialogueUI();
    //    isResponse = true;
    //    StartCoroutine(StepThroughDialogue(keyNum));
    //}

    //public void ShowDialogue(int keyNum)
    //{
    //    OpenDialogueUI();
    //    StartCoroutine(StepThroughDialogue(keyNum));
    //}
   
    //private IEnumerator StepThroughDialogue(int keyNum)
    //{
    //    if (keyNum == 0)
    //    {
    //        yield break;
    //    }
    //    while (true)
    //    {
    //        string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
    //        string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;
    //        string[] contexteArray = DialogueDBManager.instance.dialogueDic[keyNum].contextes;

    //        for (int i = 0; i < contexteArray.Length; i++)
    //        {
    //            string dialogue = DialogueDBManager.instance.dialogueDic[keyNum].contextes[i];
    //            dialogueBox.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
    //                DialogueDBManager.instance.dialogueDic[keyNum].speakerName.Trim();
    //            //yield return myTextEffect.Run(dialogue, dialogueText);
    //            yield return RunTypingEffect(dialogue);
                
    //            dialogueText.text = dialogue;

    //            yield return null;
    //            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
    //        }
            
    //        if(isResponse)
    //        {
    //            //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
    //            OpenTutoQuestion();
    //        }

    //        if (nextId != "")
    //        {
    //            keyNum = int.Parse(nextId);
    //        }
    //        else if (nextId == "")
    //        {
    //            // TODO 대화창 닫고 플레이어 이동제한 해제
    //            Debug.Log("여기온거면 끝난거임");
    //            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
    //            if (isResponse == false)
    //            {
    //                CloseDialogueUI();
    //                //플레이어 움직임 다시 제어 // TODO 캐릭터 움직임 제어
    //                //testPlayer.isMove = true;
    //            }
    //            break;
    //        }
            
    //        Debug.Log("이건 마지막 eKey 위에 디버그");
    //    }
    //    yield break;
    //}
    #endregion
    private IEnumerator RunTypingEffect(string dialouge, int voice)
    {
        //int toneNum = voice;
        myTextEffect.Run(dialouge, dialogueText, myAduio, voice); //매직넘버 톤리스트 결정하는거임
        while (myTextEffect.isTypingRunning)
        {
            yield return null;
            if(Input.GetKeyDown(KeyCode.E))
            {
                myTextEffect.Stop();
                myTextEffect.fadeImgae.SetActive(true);
            }
        }
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

    //12.12 추가 
    private IEnumerator StepThroughStateDialogue(int keyNum)
    {
        if (keyNum == 0)
        {
            yield break;
        }
        while (true)
        {
            string id = DialogueDBManager.instance.statusDialogueDic[keyNum].dialogueID;
            string nextId = DialogueDBManager.instance.statusDialogueDic[keyNum].nextTextNum;
            string[] contexteArray = DialogueDBManager.instance.statusDialogueDic[keyNum].contextes;
            int voice = int.Parse(DialogueDBManager.instance.statusDialogueDic[keyNum].voice);

            for (int i = 0; i < contexteArray.Length; i++)
            {
                string dialogue = DialogueDBManager.instance.statusDialogueDic[keyNum].contextes[i];
                dialogueBox.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    DialogueDBManager.instance.statusDialogueDic[keyNum].speakerName.Trim();
                //yield return myTextEffect.Run(dialogue, dialogueText);
                //Debug.Log(dialogue);
                //만약 대화ID당 한사람만 말하는 것이 아니라면 이 안에다가 voice 를 넣어줘야할듯
                yield return RunTypingEffect(dialogue, voice);

                dialogueText.text = dialogue;
                myTextEffect.fadeImgae.SetActive(true);

                yield return null;
                //Debug.Log("E키 누르기 전인데 실행될려나?");

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                //Debug.Log("E키 누른뒤인데 작동 안하나?");
                myTextEffect.fadeImgae.SetActive(false);
            }

            if (isResponse)
            {
                //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
                OpenTutoQuestion();
            }

            if (nextId != "" && nextId != null)
            {
                Debug.LogFormat("{0}<== 여기가 nextId에 값이 들어있을때인데 무슨 값이죠?", nextId);
                keyNum = int.Parse(nextId);
            }
            else if (nextId == "" || nextId == null)
            {
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                myTextEffect.fadeImgae.SetActive(false);
                if (isResponse == false)
                {
                    CloseDialogueUI();
                    //플레이어 움직임 다시 제어
                    //testPlayer.isMove = true;
                    playerInteract.isInteract = true; //다시 대화할 수 있게끔
                    //23.12.26 
                    PlayerInteractNpc.isTalking = false; // 대화 끝난거확인 
                    Controller_Physics.SwitchCameraLock(false);
                }
                break;
            }

            Debug.Log("이건 마지막 eKey 위에 디버그");
        }
        yield break;
    }

    public void ShowStateTutorialDialogue(int keyNum)
    {
        OpenDialogueUI();
        isResponse = true;
        StartCoroutine(StepThroughStateDialogue(keyNum));
    }

    public void ShowStateDialogue(int keyNum)
    {
        OpenDialogueUI();
        StartCoroutine(StepThroughStateDialogue(keyNum));
    }

    public void CheckStateDialogue(int keyNum, npcState _state)
    {
        //status
        // 0 = 기본적인 상태
        // 1 = 접착제 묻은 상태
        // 2 = 물건 붙은 상태

        if (_state == npcState.objectAttached)
        {
            
            ShowStateDialogue(keyNum + 2);
        }
        else if (_state == npcState.glued)
        {
            ShowStateDialogue(keyNum + 1);
        }
        else
        {
            ShowStateDialogue(keyNum);
        }
    }
    public void CheckStateTutorialDialogue(npcState _state)
    {
        //status
        // 0 = 기본적인 상태
        // 1 = 접착제 묻은 상태
        // 2 = 물건 붙은 상태

        if (_state == npcState.objectAttached)
        {
            ShowStateDialogue(12);
        }
        else if (_state == npcState.glued)
        {
            ShowStateDialogue(11);
        }
        else
        {
            ShowStateTutorialDialogue(1); // 토끼 첫번째 대사 
        }
    }

    //폰트 사이즈 변경
    public void ChangeFontSize(float fontSize)
    {
        float changeSize = fontSize;
        if(fontSize <= minFontSize)
        {
            Debug.Log("만약 폰트 사이즈가 최소 사이즈 보다 작다면");
            changeSize = minFontSize;
            Debug.LogFormat("{0}<< 변경 전 폰트 사이즈", dialogueText.fontSize);
            dialogueText.fontSize = changeSize;
            Debug.LogFormat("{0}<< 변경 후 폰트 사이즈", dialogueText.fontSize);
        }
        else if( fontSize >= maxFontSize)
        {
            Debug.Log("만약 폰트 사이즈가 max 사이즈 보다 크다면");
            changeSize = maxFontSize;
            Debug.LogFormat("{0}<< 변경 전 폰트 사이즈", dialogueText.fontSize);
            dialogueText.fontSize = changeSize;
            Debug.LogFormat("{0}<< 변경 후 폰트 사이즈", dialogueText.fontSize);
        }
        else
        {
            Debug.Log("그 외 사이즈");
            changeSize = fontSize;
            Debug.LogFormat("{0}<< 변경 전 폰트 사이즈", dialogueText.fontSize);
            dialogueText.fontSize = changeSize;
            Debug.LogFormat("{0}<< 변경 후 폰트 사이즈", dialogueText.fontSize);
        }
    }

    //public void ChangeMinOrMaxSize()
    //{

    //}
}
