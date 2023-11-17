using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValue : MonoBehaviour
{
    public float moveSpeed = 5;
    public float rotateSpeed = 5;
    public float jumpForce = 5;
    public float slowSpeed = 0.5f;

    private bool checkGround;
    private bool checkGrabObject;
    private bool checkUmbrellaOpen;
    private bool checkMoveToObject;
    public bool CheckGround { get { return checkGround; } set { checkGround = value; } }
    public bool CheckGrabObject { get { return checkGrabObject; } set { checkGrabObject = value; } }
    public bool CheckUmbrellaOpen { get { return checkUmbrellaOpen; } set { checkUmbrellaOpen = value; } }
    public bool CheckMoveToObject { get { return checkMoveToObject; } set { checkMoveToObject = value; } }

    public PlayerState stateMachine;

    private void Awake()
    {
        checkGround = true;
        checkGrabObject = false;
        checkUmbrellaOpen = false;
        checkMoveToObject = false;
        stateMachine = PlayerState.IDLE;
    }
}

public enum PlayerState
{
    IDLE, JUMP, GRAB, STOP
}
