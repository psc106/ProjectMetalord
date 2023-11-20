using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseTest : MonoBehaviour, IInteractObject
{
    public ItemStatusTest itemStatus;
    public Rigidbody itemRigidbody;


    private void Awake()
    {
        itemRigidbody = GetComponent<Rigidbody>();
    }


    public virtual void Interact(PlayerValue player)
    {
        if (player.playerState == PlayerState.GRAB)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public virtual void PopupIcon(){}
}
