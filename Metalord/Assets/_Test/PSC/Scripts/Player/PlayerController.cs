using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //IDLE =0, GRAB, JUMP, FALL, UMBRELLA, SYSTEM_STOP
    float[] moveMultiple = { 1, .7f, .4f, 0, .3f, 0f };

    //입력 데이터
    private Vector2 moveValue;
    private Vector2 preMoveDir;
    private bool jumpTrigger;
    private bool interactTrigger;


    //인풋 시스템
    [SerializeField]
    PlayerValue playerValue;

    //인풋 시스템 리더
    [SerializeField]
    private InputReader reader;

    //기본 컴퍼넌트
    [SerializeField]
    private Rigidbody playerRigid;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Collider FrontCheckCollider;
    [SerializeField]
    private Collider BodyCollider;

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

        //입력 초기화
        moveValue = Vector2.zero;
        preMoveDir = -Vector3.forward;
        jumpTrigger = false;
        interactTrigger = false;
        playerValue.constantForce = gameObject.AddComponent<ConstantForce>();
    }

    private void Start()
    {
        BindHandler();
    }

    private void Update()
    {
        CheckGround();
        Move();
        Jump();
        Fall();
        Interact();
        LookDirection();
    }


    void LookDirection()
    {
        float angle = Mathf.Atan2(preMoveDir.x, preMoveDir.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.rotateSpeed);

        playerRigid.transform.rotation = smoothRotation;

    }


    void Move()
    {
        float multiple = moveMultiple[(int)playerValue.playerState];

        //키 안누를 경우
        if (moveValue == Vector2.zero)
        {
            MoveEnd();
            return;
        }

        //키 누를경우
        preMoveDir = moveValue;
        Vector2 input = moveValue * playerValue.moveSpeed * multiple;
        Debug.Log(input);
        playerRigid.velocity = new Vector3(input.x, playerRigid.velocity.y, input.y);
    }

    void MoveEnd()
    {
        Vector3 nonY = playerRigid.velocity;
        nonY.y = 0;

        if (nonY.magnitude >= 0.005f)
        {
            playerRigid.velocity = nonY * playerValue.slowSpeed + Vector3.up * playerRigid.velocity.y;
        }
    }

    void Jump()
    {
        if (jumpTrigger)
        {
            Debug.Log("점프");
            if (playerValue.playerState == PlayerState.IDLE)
            {
                playerValue.playerState = PlayerState.JUMP;
                playerRigid.AddForce(Vector3.up * playerValue.jumpForce, ForceMode.Impulse);
                playerValue.CheckGround = (false);
                jumpTrigger = false;
                Invoke("CheckGroundRay", 1);
                playerValue.constantForce.force = new Vector3(0, -1, 0);
            }
            else if (playerValue.playerState == PlayerState.FALL)
            {
                Debug.Log("우산");
                playerValue.playerState = PlayerState.UMBRELLA;
                jumpTrigger = false;
            }
        }
    }

    void Fall()
    {
        if (playerValue.playerState == PlayerState.UMBRELLA)
        {
            Vector3 gravityDown = playerRigid.velocity;
            gravityDown.y = -1f;
            playerRigid.velocity = gravityDown;
        }
    }

    void Interact()
    {
        if (interactTrigger)
        {
            Debug.Log("상호작용키");
            interactTrigger = false;
        }
    }

    void CheckGround()
    {
        if (playerValue.playerState == PlayerState.IDLE || playerValue.playerState == PlayerState.GRAB)
        {
            CheckGroundRay();
        }
    }

    void CheckGroundRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerRigid.transform.position + Vector3.up, Vector3.down, out hit, 1000))
        {
            if (hit.distance >= 7.5f)
            {
                playerValue.CheckGround = false;
                playerValue.playerState = PlayerState.FALL;
                playerValue.constantForce.force = new Vector3(0, -1, 0);
            }
        }
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

    private void BindHandler()
    {
        reader.MoveEvent += HandleMove;

        reader.JumpEvent += HandleJump;
        reader.JumpCancelEvent += HandleJumpCancel;

        reader.InteractEvent += HandleInteract;
        reader.InteractCancelEvent += HandleInteractCancel;
    }

}
