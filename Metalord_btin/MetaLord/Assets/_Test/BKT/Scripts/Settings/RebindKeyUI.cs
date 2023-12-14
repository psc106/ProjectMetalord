using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActions
{
    Move,
    Jump,
    Fire,
    Pull,
    Run,
}

public enum BindingKeyword
{
    Forward = 1,
    Back = 2,
    Left = 3,
    Right = 4,
    Run = 0,
    Jump = 0,
    Fire = 0,
    Pull = 0,
}

/// <summary>
/// 런타임에 Key를 리바인딩 하기위한 클래스
/// 231214 배경택
/// </summary>
public class RebindKeyUI : MonoBehaviour
{
    public InputReader inputReader; // 플레이어 인풋 리더 스크립터블 오브젝트를 가져오기위한 변수
    private InputAction gameInputAction;
    [SerializeField] private PlayerActions playerActions; // 제어하고자 하는 액션
    [SerializeField] private BindingKeyword bindingKeyword; // 제어하고자하는 세부 액션 _ bindingIndex가 반영되어있습니다.
    [SerializeField] private GameObject wait_Text; // 키 입력받는동안 표시될 게임오브젝트
    [SerializeField] private GameObject input_Text; // 키 입력이 들어와있는 게임오브젝트


    //public PlayerActions playerActions = PlayerActions.Forward;

    private void Awake()
    {
        // 인스펙터창에서 Enum을 통해 제어할 플레이어 액션을 선택
        switch (playerActions)
        {
            case PlayerActions.Move:
                gameInputAction = inputReader.inputActions.Player.Move;
                break;
            case PlayerActions.Jump:
                gameInputAction = inputReader.inputActions.Player.Jump;
                break;
            case PlayerActions.Fire:
                gameInputAction = inputReader.inputActions.Player.Fire;
                break;
            case PlayerActions.Pull:
                gameInputAction = inputReader.inputActions.Player.Pull;
                break;
            case PlayerActions.Run:
                gameInputAction = inputReader.inputActions.Player.Run;
                break;
            default:
                break;
        }

    }

    // 바인딩 버튼을 누를때 호출되는 버튼
    public void onClickBindingButton()
    {
        gameInputAction.Disable(); // Action이 활성화 되어있을경우 리바인딩을 할 수 없어서 Disable해줌

        input_Text.SetActive(false);
        wait_Text.SetActive(true);

        Debug.Log("입력 들어옴");
        gameInputAction.PerformInteractiveRebinding() // 제어할 액션을 유저 상호작용형으로 리바인딩
                    .WithTargetBinding((int)bindingKeyword) // 바인딩 인덱스 적용
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnCancel(operation =>
                    {
                        gameInputAction.Enable();
                        input_Text.SetActive(true);
                        wait_Text.SetActive(false);
                    }) // 취소시 Action Enable
                    .OnComplete(operation =>
                    {
                        gameInputAction.Enable();
                        input_Text.SetActive(true);
                        wait_Text.SetActive(false);
                    }) // 완료시 Action Enable
                    .Start();
    }



}
