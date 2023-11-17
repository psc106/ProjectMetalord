using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValue : MonoBehaviour
{
    public float moveSpeed = default;
    public float rotateSpeed = default;
    public float jumpForce = default;
    public float slowSpeed = default;

    private bool checkGround;
    public bool CheckGround { get { return checkGround; } set { checkGround = value; } }


    public PlayerState playerState;

    public GameObject interactObject;

    public Transform leftHandsHook;
    public Transform rightHandsHook;

    public Transform openUmbrella;
    public Transform closeUmbrella;
    public ConstantForce constantForce;

    private void Awake()
    {
        checkGround = true;
        playerState = PlayerState.IDLE;
    }
}

public enum PlayerState
{
    IDLE =0, GRAB, JUMP, FALL, UMBRELLA, SYSTEM_STOP
}
