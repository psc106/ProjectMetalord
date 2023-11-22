using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;


public class PlayerController : MonoBehaviour
{
    private Vector3 preMoveDir;

    //플레이어가 가진 값들
    [SerializeField]
    PlayerValue playerValue;

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

        preMoveDir = -Vector3.forward;
    }


    private void Update()
    {
        /*CheckItemDistance();

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
        Interact();*/

        CheckEnviroment();
        Move();
        LookDirection();
        Jump();

    }

    RaycastHit groundHit;

    void CheckEnviroment()
    {
        //슬로프 체크
        if(Physics.Raycast(playerRigid.position+Vector3.up, Vector3.down, out groundHit, 2f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, groundHit.normal);
            if ((angle != 0 && angle <= 45))
            {
                playerValue.checkSlope = true;
                playerRigid.useGravity = false;
            }
            else
            {
                playerValue.checkSlope = false;
                playerRigid.useGravity = true;
            }
            return;
        }
        playerValue.checkSlope = false;
        playerRigid.useGravity = true;


        //점프 상태 체크
        if (Physics.Raycast(playerRigid.position + Vector3.up, Vector3.down, out groundHit, 5))
        {
            return;
        }

        //공중 상태
        if (playerValue.playerState == PlayerState.IDLE &&
            playerValue.playerState == PlayerState.GRAB &&
            playerValue.playerState == PlayerState.JUMP)
        {
            playerValue.playerState = PlayerState.FALL;
        } 

    }

    void Move()
    {
        //상태에 따른 이동속도 계수
        float multiple = playerValue.GetMoveMultiple();

        //이동키를 뗄 경우
        if (playerValue.moveValue == Vector2.zero)
        {
            if (playerValue.playerState == PlayerState.IDLE || playerValue.playerState == PlayerState.GRAB)
            {
                playerRigid.velocity = Vector3.zero + Vector3.up * playerRigid.velocity.y;
                if (playerValue.checkSlope)
                {

                    if (playerRigid.velocity.y > 0)
                    {
                        playerRigid.AddForce(Vector3.down * 100, ForceMode.Force);
                    }
                }
            }
            //천천히 멈춘다.
            return;
        }

        //이동키 누를 경우


        //기본 속도 + 이동속도 계수 계산
        Vector2 input = playerValue.moveValue ;
        //적용


        
        Vector3 cameraForward = (playerValue.oriented.forward * input.y).normalized;
        Vector3 cameraRight = (playerValue.oriented.right * input.x).normalized ;

        Vector3 inputDirection = (cameraForward + cameraRight).normalized * playerValue.moveSpeed * multiple;
        if (playerValue.checkSlope)
        {
            Vector3 slopeAngle = Vector3.ProjectOnPlane(inputDirection, groundHit.normal).normalized;
            playerRigid.velocity = (slopeAngle * playerValue.moveSpeed * multiple);
            if (playerRigid.velocity.y > 0)
            {
            }
            preMoveDir = slopeAngle;
        }
        else
        {
            playerRigid.velocity = inputDirection + (Vector3.up * playerRigid.velocity.y);
            preMoveDir = inputDirection;
        }

        if (playerValue.playerState == PlayerState.GRAB)
        {
            playerValue.interactObject.itemRigidbody.velocity = playerRigid.velocity;
            float currDistance = Vector3.Distance(playerValue.interactObject.transform.position, playerRigid.position);
            if (currDistance >= playerValue.grabDistance)
            {
                //                playerValue.interactObject.itemRigidbody.Move();
            }
        }
    }
    void LookDirection()
    {
        //lerp를 사용하여 부드럽게 회전시킨다.
        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, Quaternion.LookRotation(preMoveDir), Time.deltaTime * playerValue.rotateSpeed);

        //적용
        playerRigid.rotation = smoothRotation;



        if (playerValue.playerState == PlayerState.GRAB)
        {
            Vector3 player = playerRigid.position;
            Vector3 item = playerValue.interactObject.transform.position;
            player.y = item.y;

            //playerValue.interactObject.transform.LookAt(player);
            playerRigid.transform.LookAt(item);
        }
    }

    void Jump()
    {
        if (playerValue.jumpTrigger)
        {
            playerValue.jumpTrigger = false;
            //기본 상태 - 점프
            if (playerValue.playerState == PlayerState.IDLE)
            {
                playerValue.playerState = PlayerState.JUMP;
                playerRigid.AddForce(Vector3.up * playerValue.jumpForce, ForceMode.Impulse);
                playerValue.checkGround = (false);
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
/*

    void LookDirection()
    {
        //저장된 입력값을 기반으로 방향을 설정한다.
        float angle = Mathf.Atan2(preMoveDir.x, preMoveDir.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        //lerp를 사용하여 부드럽게 회전시킨다.
        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.rotateSpeed);

        //적용
        playerRigid.rotation = smoothRotation;



        if (playerValue.playerState == PlayerState.GRAB)
        {
            Vector3 player = playerRigid.position;
            Vector3 item = playerValue.interactObject.transform.position;
            player.y = item.y;

            //playerValue.interactObject.transform.LookAt(player);
            playerRigid.transform.LookAt(item);
        }
    }


    void CheckItemDistance()
    {
        if (playerValue.playerState == PlayerState.GRAB)
        {
            if (Mathf.Abs(playerValue.interactObject.transform.position.y - playerRigid.position.y) > 6)
            {
                Debug.Log("높이로 인한 해제 : "+ Mathf.Abs(playerValue.interactObject.transform.position.y - playerRigid.position.y));
                playerValue.playerState = PlayerState.IDLE;
                playerValue.interactObject.Interact(playerValue);
            }
            if(Vector3.Distance(playerValue.interactObject.transform.position-Vector3.up* playerValue.interactObject.transform.position.y,
                                playerRigid.position - Vector3.up * playerRigid.position.y) >= 6)
            {
                Debug.Log("이동 거리로 인한 해제 : "+ Vector3.Distance(playerValue.interactObject.transform.position - Vector3.up * playerValue.interactObject.transform.position.y,
                                playerRigid.position - Vector3.up * playerRigid.position.y));
                playerValue.playerState = PlayerState.IDLE;
                playerValue.interactObject.Interact(playerValue);
            }
        }
    }


    void Move()
    {
        //상태에 따른 이동속도 계수
        float multiple = playerValue.GetMoveMultiple();

        //이동키를 뗄 경우
        if (playerValue.moveValue == Vector2.zero)
        {
            if (playerValue.playerState == PlayerState.IDLE || playerValue.playerState==PlayerState.GRAB)
            {
                StopSmooth();
            }
            //천천히 멈춘다.
            return;
        }

        //이동키 누를 경우

        //입력 방향 저장
        preMoveDir = playerValue.moveValue;

        //기본 속도 + 이동속도 계수 계산
        Vector2 input = playerValue.moveValue * playerValue.moveSpeed * multiple;
        //적용
        playerRigid.velocity = new Vector3(input.x, playerRigid.velocity.y, input.y);

        if (playerValue.checkSlope)
        {
            Vector3 slopeAngle = Vector3.ProjectOnPlane(playerRigid.velocity, hit.normal).normalized;
            playerRigid.velocity = slopeAngle * playerValue.moveSpeed * multiple;
        }

        if (playerValue.playerState == PlayerState.GRAB)
        {
            playerValue.interactObject.itemRigidbody.velocity = playerRigid.velocity;
            float currDistance = Vector3.Distance(playerValue.interactObject.transform.position, playerRigid.position);
            if (currDistance >= playerValue.grabDistance)
            {
                //                playerValue.interactObject.itemRigidbody.Move();
            }
        }
    }


    //이동키가 입력 안될시 멈추는 함수
    //현재 입력된 속도를 재적용한다.
    void StopSmooth()
    {
        //y축 속도는 고려하지않는다.
        playerRigid.velocity = Vector3.zero;
    }

    void Jump()
    {
        //점프 입력이 들어온 경우
        if (playerValue.jumpTrigger)
        {
            playerValue.jumpTrigger = false;
            //기본 상태 - 점프
            if (playerValue.playerState == PlayerState.IDLE)
            {
                playerValue.playerState = PlayerState.JUMP;
                playerRigid.AddForce(Vector3.up * playerValue.jumpForce, ForceMode.Impulse);
                playerValue.checkGround = (false);
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
        if (playerValue.interactTrigger)
        {
            Debug.Log("상호작용키");
            playerValue.interactTrigger = false;
            if (playerValue.playerState == PlayerState.IDLE)
            {
                if (playerValue.interactObject != null)
                {
                    playerValue.playerState = PlayerState.GRAB;
                    playerValue.interactObject.Interact(playerValue);
                    playerValue.grabDistance = Vector3.Distance(playerValue.interactObject.transform.position, playerRigid.position);
                }

            }
            else if (playerValue.playerState == PlayerState.GRAB)
            {
                if (playerValue.interactObject != null)
                {
                    playerValue.playerState = PlayerState.IDLE;
                    playerValue.interactObject.Interact(playerValue);
                    playerValue.grabDistance = 0;
                }

            }


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

    RaycastHit hit;

    //레이캐스트 진행후 상태 판별
    //update나 jump후 해당 함수를 호출한다.
    void CheckAirRay()
    {
        if (Physics.Raycast(playerRigid.transform.position + Vector3.up, Vector3.down, out hit, 100))
        {
            if (playerValue.playerState != PlayerState.JUMP)
            {
                if(hit.distance < 3f)
                {
                    playerValue.checkSlope = true;
                    return;
                }

            }
            //점프 높이 안됨
            if (hit.distance < 7.5f)
            {
                playerValue.checkSlope = false;
                return;
            }

            //적정 범위
            else
            {
                playerValue.checkSlope = false;
                playerValue.checkGround = false;
                playerValue.playerState = PlayerState.FALL;
                playerValue.extraGravity.enabled = true;
                return;

            }
        }

        //범위 벗어날 경우
        playerValue.checkSlope = false;
        playerValue.checkGround = false;
        playerValue.playerState = PlayerState.FALL;
        playerValue.extraGravity.enabled = true;

    }*/

}
