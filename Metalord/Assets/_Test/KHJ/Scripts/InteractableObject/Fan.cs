using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Debug.Log("선풍기 Interact 메서드 실행됨");

    }
}
