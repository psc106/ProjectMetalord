using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class ItemBaseTest : MonoBehaviour, IInteractObject
{
    public ItemStatusTest itemStatus;
    public Rigidbody itemRigidbody;


    private void Awake()
    {
        itemRigidbody = GetComponent<Rigidbody>();
    }


    public virtual void Interact(Legacy_PlayerValue player)
    {
        if (player.playerState == PlayerStateName.GRAB)
        {
            GetComponent<Renderer>().material.color = Color.green;
            itemRigidbody.mass  = 2;
            itemRigidbody.angularDrag = 20;
            
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
            itemRigidbody.mass  = 1000.0f;

            player.interactObject = null;
        }
    }

    public virtual void PopupIcon(){}
}
