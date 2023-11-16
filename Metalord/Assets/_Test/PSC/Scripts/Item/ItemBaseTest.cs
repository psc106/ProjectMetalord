using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseTest : MonoBehaviour, IInteractObject
{
    public ItemStatusTest itemStatus;


    public virtual void Interact(PlayerValue player)
    {
        GetComponent<Renderer>().material.color = Color.green;
        transform.parent = player.transform;
    }

    public virtual void PopupIcon(){}
}
