using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

enum BindKey
{
    FORWARD = 0,
    BACK,
    RIGHT,
    LEFT,
    RUN,
    JUMP,
    SHOOT,
    PULL
}

/// <summary>
/// 키 리바인딩 클래스
/// 231212 배경택
/// </summary>
public class RebindKey : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null; // 플레이어 컨트롤러

    [SerializeField] private InputActionReference moveAction = null; // Move 액션에 대한 참조
    [SerializeField] private InputActionReference jumpAction = null; // jump 액션에 대한 참조
    [SerializeField] private InputActionReference runAction = null; // Run 액션에 대한 참조
    [SerializeField] private InputActionReference shootAction = null; // shoot 액션에 대한 참조
    [SerializeField] private InputActionReference pullAction = null; // pull 액션에 대한 참조

    [SerializeField] private GameObject settingsButton = null; // 셋팅 버튼들 모아둔 게임오브젝트, 루트오브젝트로 정하여 하위 버튼 밑 텍스트 컴포넌트 가져옴

    private Dictionary<BindKey, TMP_Text> bindingTexts = new Dictionary<BindKey, TMP_Text>();
    private Dictionary<BindKey, Button> actionButtons = new Dictionary<BindKey, Button>();

    private const string WAIT_INPUT_TEXT = "Waiting for input..."; // 입력 기다리는동안 표시될 텍스트

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation; // 다시 바인딩

    void Start()
    {
        CachingBindingTextAndButton(); // 시작시 버튼과 텍스트 캐싱
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveKey()
    {
        string keyRebinds = playerController.playerInput.actions.SaveBindingOverridesAsJson();
        Debug.Log(keyRebinds);
        //TODO 따로 외부 파일에 저장하는거 구현
    }

    public void StartRebinding(string buttonName)
    {
        
    }

    /// <summary>
    /// 텍스트 컴포넌트와 버튼 컴포넌트 캐싱하는 함수
    /// 231212 배경택
    /// </summary>
    private void CachingBindingTextAndButton()
    {
        for (int i = 0; i < (int)BindKey.PULL+1; i++)
        {
            Debug.Log((BindKey)i);
            BindKey currentKey = (BindKey)i;

            TMP_Text bindingText = settingsButton.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            Button actionButton = settingsButton.transform.GetChild(i).GetComponent<Button>();

            bindingTexts[currentKey] = bindingText;
            actionButtons[currentKey] = actionButton;

            Debug.Log(bindingTexts[currentKey].gameObject.name);
            Debug.Log(actionButtons[currentKey].gameObject.name);
        }
    }
}
