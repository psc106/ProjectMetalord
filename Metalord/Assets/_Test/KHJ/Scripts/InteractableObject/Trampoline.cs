using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Debug.Log("트램펄린 Interact 메서드 실행됨");
    }
}
