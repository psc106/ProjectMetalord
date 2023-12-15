using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractNpc : MonoBehaviour
{
    //npc 상호작용관련
    public bool isInteract = false;
    IInteractNpc playerInteract = null;
    public bool isMove = true;
    // Start is called before the first frame update
    void Start()
    {
        isMove = true;
        isInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteract)
        {
            PushE();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //IInteractableObject interObj =other.GetComponent<IInteractableObject>();
        //interObj?.Interact(); 
        
            playerInteract = other.GetComponent<IInteractNpc>();
            isInteract = true;
        

    }

    private void OnTriggerExit(Collider other)
    {
        //IInteractableObject interObj = other.GetComponent<IInteractableObject>();
        //interObj?.InteractOut();
        
            playerInteract = null;
            isInteract = false;
        
    }
    public void PushE()
    {
        if (playerInteract != null && Input.GetKeyDown(KeyCode.E))
        {
            isMove = false;
            isInteract = false;
            Debug.Log("플레이어 E 키누르기 ");
            playerInteract.InteractNpc();
        }
    }
}
