using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranpolineControl : MonoBehaviour, IInteractableObject
{
    Trampoline myTrampoline = default;
    // Start is called before the first frame update
    void Start()
    {
        myTrampoline = transform.parent.GetComponent<Trampoline>();
    }
  
    public void Interact()
    {
        myTrampoline.TouchPad();
    }
    public void InteractOut()
    {
        myTrampoline.ChangeOriginSize();
    }

}
