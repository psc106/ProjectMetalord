using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceContainer : MonoBehaviour, IInteractableObject
{
   
    public void Interact()
    {
        Debug.Log("소스통 Interact 메서드 실행됨");
    }
}
