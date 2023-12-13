//using UnityEngine;
//using UnityEngine.Windows;
//using UnityEngine.InputSystem;


//public class PlayerController : MonoBehaviour
//{
//    private Vector2 preMoveDir;

//    //플레이어가 가진 값들
//    [SerializeField]
//    PlayerValue playerValue;

//    public PlayerInput playerInput;

//    //기본 컴퍼넌트
//    [SerializeField]
//    private Rigidbody playerRigid;
//    [SerializeField]
//    private Animator playerAnimator;
//    [SerializeField]
//    private Collider FrontCheckCollider;
//    [SerializeField]
//    private Collider BodyCollider;

//    private void Awake()
//    {
//        if (playerRigid == null)
//        {
//            playerRigid = GetComponentInChildren<Rigidbody>();
//        }
//        if (playerAnimator == null)
//        {
//            playerAnimator = GetComponentInChildren<Animator>();
//        }


//        if (FrontCheckCollider == null)
//        {
//            FrontCheckCollider = playerRigid.transform.Find("FrontCollider").GetComponent<Collider>();
//        }
//        if (BodyCollider == null)
//        {
//            FrontCheckCollider = playerRigid.transform.Find("BodyCollider").GetComponent<Collider>();
//        }

//        preMoveDir = -Vector3.forward;
//    }


//    private void Update()
//    {
//        CheckItemDistance();
//        //공중 체크
//        //기본 상태, 그랩 상태, 점프 상태일 경우 작동한다.
//        //아래 방향으로 레이캐스트 진행
//        //레이캐스트 이후 플레이어 상태 유지/변경
//        CheckFAir();

//        //만약 우산을 사용한 상태면 velocity 고정
//        UseUmbrella();
        
//        //이동
//        //각 단계마다 이동속도 계수가 바뀌어 적용된다.
//        //정지 상태일 경우 이동속도 계수는 0이다.
//        Move();

//        //머리 회전
//        //이동방향이 지정되면 그 방향을 바라본다.
//        LookDirection();

//        //점프
//        //땅에 있을 경우 점프
//        //공중에 있을 경우 우산
//        Jump();

//        //상호작용
//        //그랩 아이템이 있을 경우 -> 그랩
//        //기타 상호작용 아이템이 있을 경우 -> 상호작용 이벤트
//        Interact();

//    }


//    void LookDirection()
//    {
//        //저장된 입력값을 기반으로 방향을 설정한다.
//        float angle = Mathf.Atan2(preMoveDir.x, preMoveDir.y) * Mathf.Rad2Deg;
//        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

//        //lerp를 사용하여 부드럽게 회전시킨다.
//        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.RotateSpeed);

//        //적용
//        playerRigid.transform.rotation = smoothRotation;

//        if (playerValue.playerState == PlayerState.GRAB)
//        {
//            playerValue.interactObject.transform.LookAt(playerRigid.position-Vector3.up* playerRigid.position.y+Vector3.up*playerValue.interactObject.transform.position.y);
//        }
//    }

//    void CheckItemDistance()
//    {
//        if (playerValue.playerState == PlayerState.GRAB)
//        {
//            if (Mathf.Abs(playerValue.interactObject.transform.position.y - transform.position.y) > 4)
//            {
//                Debug.Log("높이로 인한 해제 : "+ Mathf.Abs(playerValue.interactObject.transform.position.y - transform.position.y));
//                playerValue.playerState = PlayerState.IDLE;
//                playerValue.interactObject.Interact(playerValue);
//            }
//            if(Vector3.Distance(playerValue.interactObject.transform.position-Vector3.up* playerValue.interactObject.transform.position.y,
//                                playerRigid.position - Vector3.up * playerRigid.position.y) >= 6)
//            {
//                Debug.Log("이동 거리로 인한 해제 : "+ Vector3.Distance(playerValue.interactObject.transform.position - Vector3.up * playerValue.interactObject.transform.position.y,
//                                playerRigid.position - Vector3.up * playerRigid.position.y));
//                playerValue.playerState = PlayerState.IDLE;
//                playerValue.interactObject.Interact(playerValue);
//            }
//        }
//    }


//    void Move()
//    {
//        //상태에 따른 이동속도 계수
//        float multiple = playerValue.GetMoveMultiple();

//        //이동키를 뗄 경우
//        if (playerValue.moveValue == Vector2.zero)
//        {
//            //천천히 멈춘다.
//            StopSmooth();
//            return;
//        }

//        //이동키 누를 경우

//        //입력 방향 저장
//        preMoveDir = playerValue.moveValue;
//        //기본 속도 + 이동속도 계수 계산
//        Vector2 input = playerValue.moveValue * playerValue.MoveSpeed * multiple;

//        //적용
//        playerRigid.velocity = new Vector3(input.x, playerRigid.velocity.y, input.y);
//        if (playerValue.playerState == PlayerState.GRAB)
//        {
//            playerValue.interactObject.itemRigidbody.velocity = playerRigid.velocity;
//        }
//    }


//    //이동키가 입력 안될시 멈추는 함수
//    //현재 입력된 속도를 재적용한다.
//    void StopSmooth()
//    {
//        //y축 속도는 고려하지않는다.
//        Vector3 nonY = playerRigid.velocity;
//        nonY.y = 0;
//        if (nonY.magnitude >= 0.005f)
//        {
//            //미리 설정한 속도를 계속 곱하여 적용
//            playerRigid.velocity = nonY * playerValue.SlowSpeed + Vector3.up * playerRigid.velocity.y;
//            if (playerValue.playerState == PlayerState.GRAB)
//            {
//                playerValue.interactObject.itemRigidbody.velocity = playerRigid.velocity;
//            }
//        }
//    }

//    void Jump()
//    {
//        //점프 입력이 들어온 경우
//        if (playerValue.jumpTrigger)
//        {
//            playerValue.jumpTrigger = false;
//            //기본 상태 - 점프
//            if (playerValue.playerState == PlayerState.IDLE)
//            {
//                playerValue.playerState = PlayerState.JUMP;
//                playerRigid.AddForce(Vector3.up * playerValue.JumpForce, ForceMode.Impulse);
//                playerValue.CheckGround = (false);
//                playerValue.extraGravity.enabled = true;
//            }

//            //공중 상태 - 우산
//            else if (playerValue.playerState == PlayerState.FALL)
//            {
//                playerValue.playerState = PlayerState.UMBRELLA;
//                playerValue.extraGravity.enabled = false;
//            }

//            //우산 상태 - 공중
//            else if (playerValue.playerState == PlayerState.UMBRELLA)
//            {
//                playerValue.playerState = PlayerState.FALL;
//                playerValue.extraGravity.enabled = true;
//            }
//        }
//    }

//    void UseUmbrella()
//    {
//        //우산 상태일 경우
//        if (playerValue.playerState == PlayerState.UMBRELLA)
//        {
//            //y축 속도를 적용한다.
//            Vector3 gravityDown = playerRigid.velocity;
//            gravityDown.y = -1f;
//            playerRigid.velocity = gravityDown;
//        }
//    }

//    void Interact()
//    {
//        if (playerValue.interactTrigger)
//        {
//            Debug.Log("상호작용키");
//            playerValue.interactTrigger = false;
//            if (playerValue.playerState == PlayerState.IDLE)
//            {
//                if (playerValue.interactObject != null)
//                {
//                    playerValue.playerState = PlayerState.GRAB;
//                    playerValue.interactObject.Interact(playerValue);
//                }

//            }
//            else if (playerValue.playerState == PlayerState.GRAB)
//            {
//                if (playerValue.interactObject != null)
//                {
//                    playerValue.playerState = PlayerState.IDLE;
//                    playerValue.interactObject.Interact(playerValue);
//                }

//            }


//        }
//    }

//    //상태 체크후 레이캐스트 진행
//    //다른 곳에서 AirRay를 판별하기 위해서 분리
//    void CheckFAir()
//    {
//        if (playerValue.playerState == PlayerState.IDLE ||
//            playerValue.playerState == PlayerState.GRAB ||
//            playerValue.playerState == PlayerState.JUMP)
//        {
//            CheckAirRay();
//        }
//    }


//    //레이캐스트 진행후 상태 판별
//    //update나 jump후 해당 함수를 호출한다.
//    void CheckAirRay()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(playerRigid.transform.position + Vector3.up, Vector3.down, out hit, 100))
//        {
//            //점프 높이 안됨
//            if (hit.distance < 7.5f)
//            {
//                return;
//            }

//            //적정 범위
//            else
//            {
//                playerValue.CheckGround = false;
//                playerValue.playerState = PlayerState.FALL;
//                playerValue.extraGravity.enabled = true;
//                return;

//            }
//        }

//        //범위 벗어날 경우
//        playerValue.CheckGround = false;
//        playerValue.playerState = PlayerState.FALL;
//        playerValue.extraGravity.enabled = true;

//    }

//}
