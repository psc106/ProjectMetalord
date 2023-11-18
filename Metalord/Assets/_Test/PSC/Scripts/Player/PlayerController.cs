using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //IDLE =0, GRAB, JUMP, FALL, UMBRELLA, SYSTEM_STOP
    float[] moveMultiple = { 1, .7f, .2f, 0, .3f, 0f };

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
    }

    private void Start()
    {
        //키-함수 바인드
        BindHandler();
    }

    private void Update()
    {
        //공중 체크
        //기본 상태, 그랩 상태, 점프 상태일 경우 작동한다.
        //아래 방향으로 레이캐스트 진행
        //레이캐스트 이후 플레이어 상태 유지/변경
        CheckFAir();

        //만약 우산을 사용한 상태면 velocity 고정
        UseUmbrella();
        
        //이동
        //각 단계마다 이동속도 계수가 바뀌어 적용된다.
        //정지 상태일 경우 이동속도 계수는 0이다.
        Move();

        //머리 회전
        //이동방향이 지정되면 그 방향을 바라본다.
        LookDirection();

        //점프
        //땅에 있을 경우 점프
        //공중에 있을 경우 우산
        Jump();

        //상호작용
        //그랩 아이템이 있을 경우 -> 그랩
        //기타 상호작용 아이템이 있을 경우 -> 상호작용 이벤트
        Interact();

    }


    void LookDirection()
    {
        //저장된 입력값을 기반으로 방향을 설정한다.
        float angle = Mathf.Atan2(preMoveDir.x, preMoveDir.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        //lerp를 사용하여 부드럽게 회전시킨다.
        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.rotateSpeed);

        //적용
        playerRigid.transform.rotation = smoothRotation;

    }


    void Move()
    {
        //상태에 따른 이동속도 계수
        float multiple = moveMultiple[(int)playerValue.playerState];

        //이동키를 뗄 경우
        if (moveValue == Vector2.zero)
        {
            //천천히 멈춘다.
            StopSmooth();
            return;
        }

        //이동키 누를 경우

        //입력 방향 저장
        preMoveDir = moveValue;
        //기본 속도 + 이동속도 계수 계산
        Vector2 input = moveValue * playerValue.moveSpeed * multiple;

        //적용
        playerRigid.velocity = new Vector3(input.x, playerRigid.velocity.y, input.y);
    }


    //이동키가 입력 안될시 멈추는 함수
    //현재 입력된 속도를 재적용한다.
    void StopSmooth()
    {
        //y축 속도는 고려하지않는다.
        Vector3 nonY = playerRigid.velocity;
        nonY.y = 0;
        if (nonY.magnitude >= 0.005f)
        {
            //미리 설정한 속도를 계속 곱하여 적용
            playerRigid.velocity = nonY * playerValue.slowSpeed + Vector3.up * playerRigid.velocity.y;
        }
    }

    void Jump()
    {
        //점프 입력이 들어온 경우
        if (jumpTrigger)
        {
            jumpTrigger = false;
            //기본 상태 - 점프
            if (playerValue.playerState == PlayerState.IDLE)
            {
                playerValue.playerState = PlayerState.JUMP;
                playerRigid.AddForce(Vector3.up * playerValue.jumpForce, ForceMode.Impulse);
                playerValue.CheckGround = (false);
                playerValue.extraGravity.enabled = true;
            }

            //공중 상태 - 우산
            else if (playerValue.playerState == PlayerState.FALL)
            {
                playerValue.playerState = PlayerState.UMBRELLA;
                playerValue.extraGravity.enabled = false;
            }

            //우산 상태 - 공중
            else if (playerValue.playerState == PlayerState.UMBRELLA)
            {
                playerValue.playerState = PlayerState.FALL;
                playerValue.extraGravity.enabled = true;
            }
        }
    }

    void UseUmbrella()
    {
        //우산 상태일 경우
        if (playerValue.playerState == PlayerState.UMBRELLA)
        {
            //y축 속도를 적용한다.
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

    //상태 체크후 레이캐스트 진행
    //다른 곳에서 AirRay를 판별하기 위해서 분리
    void CheckFAir()
    {
        if (playerValue.playerState == PlayerState.IDLE ||
            playerValue.playerState == PlayerState.GRAB ||
            playerValue.playerState == PlayerState.JUMP)
        {
            CheckAirRay();
        }
    }


    //레이캐스트 진행후 상태 판별
    //update나 jump후 해당 함수를 호출한다.
    void CheckAirRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerRigid.transform.position + Vector3.up, Vector3.down, out hit, 100))
        {
            //점프 높이 안됨
            if (hit.distance < 7.5f)
            {
                return;
            }

            //적정 범위
            else
            {
                playerValue.CheckGround = false;
                playerValue.playerState = PlayerState.FALL;
                playerValue.extraGravity.enabled = true;
                return;

            }
        }

        //범위 벗어날 경우
        playerValue.CheckGround = false;
        playerValue.playerState = PlayerState.FALL;
        playerValue.extraGravity.enabled = true;

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
