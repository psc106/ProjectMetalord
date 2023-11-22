using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCollision : MonoBehaviour
{
    [SerializeField]
    PlayerValue playerValue;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerValue.checkGround = true;
            playerValue.extraGravity.enabled = false;
            playerValue.playerState = PlayerState.IDLE;
        }

    }
   /* private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerValue.checkGround = false;
            playerValue.extraGravity.enabled = true;
            playerValue.playerState = PlayerState.JUMP;
        }

    }*/




}
