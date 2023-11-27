using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptObject : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Debug.Log("방해 오브젝트 Interact 메서드 실행됨");

    }

    public void InteractOut()
    {
    }
}
