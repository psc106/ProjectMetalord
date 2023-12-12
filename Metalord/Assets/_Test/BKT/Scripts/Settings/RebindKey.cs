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

    [SerializeField] private GameObject settingsButton = null; // 셋팅 버튼들 모아둔 게임오브젝트 

    private TMP_Text forwardBindingText = null; // 전진 바인딩 표시 텍스트
    private TMP_Text backBindingText = null; // 후진 바인딩 표시 텍스트
    private TMP_Text rightBindingText = null; // 우측 바인딩 표시 텍스트
    private TMP_Text leftBindingText = null; // 좌측 바인딩 표시 텍스트
    private TMP_Text runBindingText = null; // 달리기 바인딩 표시 텍스트
    private TMP_Text jumpBindingText = null; // 점프 바인딩 표시 텍스트
    private TMP_Text shootBindingText = null; // 발사 바인딩 표시 텍스트
    private TMP_Text pullBindingText = null; // 당기기 바인딩 표시 텍스트
    private Button forwardButton = null; // 전진 버튼
    private Button backButton = null; // 후진 버튼
    private Button rightButton = null; // 우측 버튼 
    private Button leftButton = null; // 좌측 버튼
    private Button runButton = null; // 달리기 버튼
    private Button jumpButton = null; // 점프 버튼
    private Button shootButton = null; // 발사 버튼
    private Button pullButton = null; //  당기기 버튼


    private const string WAIT_INPUT_TEXT = "Waiting for input..."; // 입력 기다리는동안 표시될 텍스트

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation; // 다시 바인딩

    // Start is called before the first frame update
    void Start()
    {
        forwardBindingText = settingsButton.transform.GetChild((int)BindKey.FORWARD).GetChild(0).GetComponent<TMP_Text>();
        forwardButton = settingsButton.transform.GetChild((int)BindKey.FORWARD).GetComponent<Button>();
        backBindingText = settingsButton.transform.GetChild((int)BindKey.BACK).GetChild(0).GetComponent<TMP_Text>();
        backButton = settingsButton.transform.GetChild((int)BindKey.BACK).GetComponent<Button>();
        rightBindingText = settingsButton.transform.GetChild((int)BindKey.RIGHT).GetChild(0).GetComponent<TMP_Text>();
        rightButton = settingsButton.transform.GetChild((int)BindKey.RIGHT).GetComponent<Button>();
        leftBindingText = settingsButton.transform.GetChild((int)BindKey.LEFT).GetChild(0).GetComponent<TMP_Text>();
        leftButton = settingsButton.transform.GetChild((int)BindKey.LEFT).GetComponent<Button>();
        runBindingText = settingsButton.transform.GetChild((int)BindKey.RUN).GetChild(0).GetComponent<TMP_Text>();
        runButton = settingsButton.transform.GetChild((int)BindKey.RUN).GetComponent<Button>();
        jumpBindingText = settingsButton.transform.GetChild((int)BindKey.JUMP).GetChild(0).GetComponent<TMP_Text>();
        jumpButton = settingsButton.transform.GetChild((int)BindKey.JUMP).GetComponent<Button>();
        shootBindingText = settingsButton.transform.GetChild((int)BindKey.SHOOT).GetChild(0).GetComponent<TMP_Text>();
        shootButton = settingsButton.transform.GetChild((int)BindKey.SHOOT).GetComponent<Button>();
        pullBindingText = settingsButton.transform.GetChild((int)BindKey.PULL).GetChild(0).GetComponent<TMP_Text>();
        pullButton = settingsButton.transform.GetChild((int)BindKey.PULL).GetComponent<Button>();
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
}
