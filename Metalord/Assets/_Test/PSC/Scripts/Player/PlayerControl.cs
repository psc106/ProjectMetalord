using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    private Vector2 currDirection;

    [SerializeField]
    PlayerValue playerValue;

    [SerializeField]
    InputActionReference moveAction;

    [SerializeField]
    private PlayerInput input;
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
        if (input == null)
        {
            input = GetComponent<PlayerInput>();
        }


        if (playerRigid == null)
        {
            playerRigid = GetComponentInChildren<Rigidbody>();
        }
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<Animator>();
            Debug.LogAssertion(playerAnimator);
        }


        if (FrontCheckCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("FrontCollider").GetComponent<Collider>();
        }
        if (BodyCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("BodyCollider").GetComponent<Collider>();
        }

        currDirection = playerRigid.transform.forward;

        //Bind();
    }

    private void FixedUpdate()
    {
        Move(moveAction.action.ReadValue<Vector2>());
    }

    private void Update()
    {
        LookDirection();
    }

    void OnJump(InputValue inputValue)
    {
        if (playerValue.isGround)
        {
            Jump();
        }
    }

    void LookDirection()
    {
        float angle = Mathf.Atan2(currDirection.x, currDirection.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        Quaternion smoothRotation = Quaternion.Slerp(playerRigid.transform.rotation, targetRotation, Time.deltaTime * playerValue.rotateSpeed);

        playerRigid.transform.rotation = smoothRotation;
    }


    void Move(Vector2 input)
    {

        if (input.magnitude <= 0)
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
        currDirection = input;
    }

    void MoveEnd(Vector3 nonY)
    {
        playerRigid.velocity = nonY * playerValue.slowSpeed + Vector3.up * playerRigid.velocity.y;
    }

    void Jump()
    {
        playerRigid.AddForce(Vector3.up* playerValue.jumpForce, ForceMode.Impulse);
        playerValue.SetGroundState(false);
    }

    public void Bind()
    {

        this.moveAction.action.performed += (callbackContext) =>
        {
        };

        this.moveAction.action.canceled += (callbackContext) =>
        {
        };
    }

}
