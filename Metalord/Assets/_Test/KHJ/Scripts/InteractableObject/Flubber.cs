using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Debug.Log("탱탱볼 Interact 메서드 실행됨");

    }

    public void InteractOut()
    {
    }
}
