using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

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
        }


        if (FrontCheckCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("FrontCollider").GetComponent<Collider>();
        }
        if (BodyCollider == null)
        {
            FrontCheckCollider = playerRigid.transform.Find("BodyCollider").GetComponent<Collider>();
        }

        //Bind();
    }



    // Update is called once per frame
    void Update()
    {
        Move(moveAction.action.ReadValue<Vector2>());
    }

    void OnJump(InputValue inputValue)
    {
        if (playerValue.isGround)
        {
            Jump();
        }
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
        playerRigid.transform.LookAt(playerRigid.transform.position + new Vector3(input.x, 0, input.y));
        
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
