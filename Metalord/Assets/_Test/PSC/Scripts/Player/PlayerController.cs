using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDir;
    private bool jumpTrigger;
    private bool interactTrigger;


    [SerializeField]
    PlayerValue playerValue;

    [SerializeField]
    private InputReader reader;

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

        moveDir = playerRigid.transform.forward;
        jumpTrigger = false;
        interactTrigger = false;

    }

    private void Start()
    {
        BindHandler();
    }

    private void FixedUpdate()
    {
        Move(moveDir);
        Jump();
    }

    private void Update()
    {
        LookDirection();
    }


    void LookDirection()
    {
        float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.rotateSpeed);

        playerRigid.transform.rotation = smoothRotation;
    }


    void Move(Vector2 input)
    {

        if (input == Vector2.zero)
        {
            Vector3 nonY = playerRigid.velocity;
            nonY.y = 0;

            if (nonY.magnitude >= 0.005f)
            {
                MoveEnd(nonY);
            }
            return;
        }

        input *= playerValue.moveSpeed;
        playerRigid.velocity = new Vector3(input.x, playerRigid.velocity.y, input.y);
        moveDir = input;
    }

    void MoveEnd(Vector3 nonY)
    {
        playerRigid.velocity = nonY * playerValue.slowSpeed + Vector3.up * playerRigid.velocity.y;
    }

    void Jump()
    {
        playerRigid.AddForce(Vector3.up* playerValue.jumpForce, ForceMode.Impulse);
        playerValue.CheckGround = (false);
    }

    void HandleMove(Vector2 dir)
    {
        moveDir = dir;
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
