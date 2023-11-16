using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValue : MonoBehaviour
{
    public float moveSpeed = 5;
    public float rotateSpeed = 5;
    public float jumpForce = 5;
    public float slowSpeed = 0.5f;
    public bool isGround { get; private set; }
    public bool isInteract { get; private set; }

    public PlayerState stateMachine;
    public GameObject interactObject;

    public Transform handPos;

    private void Awake()
    {
        isGround = true;
        stateMachine = PlayerState.IDLE;
        interactObject = null;
    }
    public void SetGroundState(bool check)
    {
        isGround = check;
    }
    public void SetInteractState(bool state)
    {
        isInteract = state;
    }



}

public enum PlayerState
{
    IDLE, JUMP, ITEM, STORY
}
