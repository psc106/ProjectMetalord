using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerValue : MonoBehaviour
{
    //IDLE, GRAB, JUMP, FALL, UMBRELLA, SYSTEM_STOP
    private float[] moveMultiple = { 1, .2f, .6f, 0.1f, .3f, 0f };
    public float GetMoveMultiple()
    {     
        return moveMultiple[(int)playerState];
    }

    //입력 데이터
    [HideInInspector]
    public Vector2 moveValue;
    [HideInInspector]
    public bool jumpTrigger;
    [HideInInspector]
    public bool interactTrigger;

    [SerializeField]
    float moveSpeed = default;
    public float MoveSpeed { get { return moveSpeed; } private set { moveSpeed = value; } }
    [SerializeField]
    float rotateSpeed = default;
    public float RotateSpeed { get { return rotateSpeed; } private set { rotateSpeed = value; } }
    [SerializeField]
    float jumpForce = default;
    public float JumpForce { get { return jumpForce; } private set { jumpForce = value; } }
    [SerializeField]
    float slowSpeed = default;
    public float SlowSpeed { get { return slowSpeed; } private set { slowSpeed = value; } }

    private bool checkGround;
    public bool CheckGround { get { return checkGround; } set { checkGround = value; } }

    private bool checkInteract;
    public bool CheckInteract { get { return checkInteract; } set { checkInteract = value; } }

    public PlayerState playerState;
    public ItemBaseTest interactObject;

    public Transform leftHandsHook;
    public Transform rightHandsHook;
    public Transform openUmbrella;
    public Transform closeUmbrella;

    public ConstantForce extraGravity;

    //인풋 시스템 리더
    [SerializeField]
    private InputReader reader;

    private void Awake()    
    {
        checkGround = true;
        checkGround = false;
        extraGravity.enabled = false;
        playerState = PlayerState.IDLE;

        //입력 초기화
        moveValue = Vector2.zero;
        jumpTrigger = false;
        interactTrigger = false;
    }

    private void Start()
    {
        //키-함수 바인드
        BindHandler();
    }



    #region 바인딩 함수
    private void BindHandler()
    {
        reader.MoveEvent += HandleMove;

        reader.JumpEvent += HandleJump;
        reader.JumpCancelEvent += HandleJumpCancel;

        reader.InteractEvent += HandleInteract;
        reader.InteractCancelEvent += HandleInteractCancel;
    }

    void HandleMove(Vector2 dir)
    {
        moveValue = dir;
    }

    private void HandleJump()
    {
        jumpTrigger = true;

    }
    private void HandleJumpCancel()
    {
        jumpTrigger = false;

    }

    private void HandleInteract()
    {
        interactTrigger = true;

    }
    private void HandleInteractCancel()
    {
        interactTrigger = false;

    }

    #endregion
}

public enum PlayerState
{
    IDLE =0, GRAB, JUMP, FALL, UMBRELLA, SYSTEM_STOP
}
