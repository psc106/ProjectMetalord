using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : StateBase<PlayerStateName>
{
    public PlayerValue playerValue;
    public Vector3 preMoveDir;

    public PlayerIdleState(PlayerStateName key) : base(key)
    {
    }
    public PlayerIdleState(PlayerStateName key, PlayerValue value) : base(key)
    {
        playerValue = value;
    }


    public override void Enter()
    {
        //throw new System.NotImplementedException();
    }

    public override void FixedUpdateFunc()
    {
        Move();
        LookDirection();
    }

    public override void UpdateFunc()
    {
        CheckEnviroment();
        Jump();
    }

    public override void LateUpdateFunc()
    {
        //throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public override PlayerStateName GetNextState()
    {
        return PlayerStateName.IDLE;
        //throw new System.NotImplementedException();
    }


    RaycastHit groundHit;

    void CheckEnviroment()
    {
        //슬로프 체크
        if (Physics.Raycast(playerValue.playerRigid.position + Vector3.up, Vector3.down, out groundHit, 2f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, groundHit.normal);
            if ((angle != 0 && angle <= 45))
            {
                playerValue.checkSlope = true;
                playerValue.playerRigid.useGravity = false;
            }
            else
            {
                playerValue.checkSlope = false;
                playerValue.playerRigid.useGravity = true;
            }
            return;
        }



        //점프 상태 체크
        if (Physics.Raycast(playerValue.playerRigid.position + Vector3.up, Vector3.down, out groundHit, 5f, LayerMask.GetMask("Ground")))
        {

            return;
        }

        //공중 상태
        if (!playerValue.checkGround &&
            (playerValue.playerState == PlayerStateName.IDLE ||
            playerValue.playerState == PlayerStateName.GRAB ||
            playerValue.playerState == PlayerStateName.JUMP))
        {
            playerValue.checkSlope = false;
            playerValue.playerRigid.useGravity = true;
            playerValue.playerState = PlayerStateName.FALL;
        }

    }

    void Move()
    {
        if (playerValue.playerState == PlayerStateName.FALL) return;

        //상태에 따른 이동속도 계수
        float multiple = playerValue.GetMoveMultiple();

        //이동키를 뗄 경우
        if (playerValue.moveValue == Vector2.zero)
        {
            if (playerValue.playerState == PlayerStateName.IDLE || playerValue.playerState == PlayerStateName.GRAB)
            {
                playerValue.playerRigid.velocity = Vector3.zero + Vector3.up * playerValue.playerRigid.velocity.y;
                if (playerValue.checkSlope && playerValue.checkJump)
                {
                    if (playerValue.playerRigid.velocity.y > 0)
                    {
                        playerValue.playerRigid.AddForce(Vector3.down * 100, ForceMode.Force);
                    }
                }
            }
            return;
        }

        //이동키 누를 경우


        //기본 속도 + 이동속도 계수 계산
        Vector2 input = playerValue.moveValue;
        //적용



        Vector3 cameraForward = (playerValue.oriented.forward * input.y).normalized;
        Vector3 cameraRight = (playerValue.oriented.right * input.x).normalized;

        Vector3 inputDirection = (cameraForward + cameraRight).normalized * playerValue.moveSpeed * multiple;
        if (playerValue.checkSlope && playerValue.checkJump)
        {
            Vector3 slopeAngle = Vector3.ProjectOnPlane(inputDirection, groundHit.normal).normalized;
            playerValue.playerRigid.velocity = (slopeAngle * playerValue.moveSpeed * multiple);
            if (playerValue.playerRigid.velocity.y > 0)
            {
                playerValue.playerRigid.AddForce(Vector3.down * 100, ForceMode.Force);
            }
            preMoveDir = inputDirection;
        }
        else
        {
            playerValue.playerRigid.velocity = inputDirection + (Vector3.up * playerValue.playerRigid.velocity.y);
            preMoveDir = inputDirection;
        }

        if (playerValue.playerState == PlayerStateName.GRAB)
        {
            playerValue.interactObject.itemRigidbody.velocity = playerValue.playerRigid.velocity;
            float currDistance = Vector3.Distance(playerValue.interactObject.transform.position, playerValue.playerRigid.position);
            if (currDistance >= playerValue.grabDistance)
            {
                //                playerValue.interactObject.itemRigidbody.Move();
            }
        }
    }
    void LookDirection()
    {
        //lerp를 사용하여 부드럽게 회전시킨다.
        Quaternion smoothRotation = Quaternion.Lerp(playerValue.playerRigid.transform.rotation, Quaternion.LookRotation(preMoveDir), Time.deltaTime * playerValue.rotateSpeed);

        //적용
        playerValue.playerRigid.rotation = smoothRotation;



        if (playerValue.playerState == PlayerStateName.GRAB)
        {
            Vector3 player = playerValue.playerRigid.position;
            Vector3 item = playerValue.interactObject.transform.position;
            player.y = item.y;

            //playerValue.interactObject.transform.LookAt(player);
            playerValue.playerRigid.transform.LookAt(item);
        }
    }

    void Jump()
    {
        if (playerValue.jumpTrigger)
        {
            playerValue.jumpTrigger = false;
            //기본 상태 - 점프
            if (playerValue.playerState == PlayerStateName.IDLE)
            {
                playerValue.checkSlope = false;
                playerValue.playerRigid.useGravity = true;
                playerValue.playerState = PlayerStateName.JUMP;
                playerValue.playerRigid.AddForce(Vector3.up * playerValue.jumpForce, ForceMode.Impulse);
                playerValue.checkGround = (false);
                playerValue.checkJump = true;
                playerValue.extraGravity.enabled = true;
            }

            //공중 상태 - 우산
            /* else if (playerValue.playerState == PlayerStateName.FALL)
             {
                 playerValue.playerState = PlayerStateName.UMBRELLA;
                 playerValue.extraGravity.enabled = false;
             }

             //우산 상태 - 공중
             else if (playerValue.playerState == PlayerStateName.UMBRELLA)
             {
                 playerValue.playerState = PlayerStateName.FALL;
                 playerValue.extraGravity.enabled = true;
             }*/
        }
    }
}
