using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Debug.Log("토스터기 Interact 메서드 실행됨");

    }
}
