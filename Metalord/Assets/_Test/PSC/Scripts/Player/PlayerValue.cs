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

    //기본 컴퍼넌트
    public Rigidbody playerRigid;
    public Animator playerAnimator;
    public Collider FrontCheckCollider;
    public Collider BodyCollider;

    //입력 데이터
    [HideInInspector]
    public Vector2 moveValue;
    [HideInInspector]
    public bool jumpTrigger;
    [HideInInspector]
    public bool interactTrigger;

    public float moveSpeed = default;
    public float rotateSpeed = default;
    public float jumpForce = default;
    public float slowSpeed = default;

    public bool checkGround;
    public bool checkInteract;
    public bool checkSlope;
    public bool checkJump;

    public PlayerStateName playerState;
    public ItemBaseTest interactObject;
    public float grabDistance;

    public Transform oriented;
    public Transform leftHandsHook;
    public Transform rightHandsHook;
    public Transform openUmbrella;
    public Transform closeUmbrella;
    public Transform closeUmbrellaEnd;

    public ConstantForce extraGravity;

    //인풋 시스템 리더
    [SerializeField]
    private InputReader reader;

    private void Awake()
    {
        if (playerRigid == null)
        {
            playerRigid = GetComponentInChildren<Rigidbody>();
        }
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<Animator>();
        }


        if (FrontCheckCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("FrontCollider").GetComponent<Collider>();
        }
        if (BodyCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("BodyCollider").GetComponent<Collider>();
        }

        checkGround = true;
        checkSlope = false;
        checkJump = false;
        extraGravity.enabled = false;
        playerState = PlayerStateName.IDLE;

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
