using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Obsolete]
public class Legacy_GroundCollision : MonoBehaviour
{
    [SerializeField]
    Legacy_PlayerValue playerValue;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerValue.checkGround = true;
            playerValue.extraGravity.enabled = false;
            playerValue.playerState = PlayerStateName.IDLE;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerValue.checkGround = false;
            playerValue.extraGravity.enabled = true;
            playerValue.playerState = PlayerStateName.JUMP;
        }

    }




}
