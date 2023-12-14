using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterButton : MonoBehaviour,IInteractableObject
{
    private Toaster pareantObject;
    // Start is called before the first frame update
    void Start()
    {
        pareantObject = transform.parent.GetComponent<Toaster>();
    }

    public void Interact()
    {
        pareantObject.UpToast();
        pareantObject.DownToast();
        //pareantObject.isToaster = true;
    }

    public void InteractOut()
    {
        //pareantObject.isToaster = false;
    }
}
