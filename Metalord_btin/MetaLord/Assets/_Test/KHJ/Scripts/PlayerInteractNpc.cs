using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractNpc : MonoBehaviour
{
    //npc 상호작용관련
    public bool isInteract = false;
    IInteractNpc playerInteract = null;
    
    void Update()
    {
        if (isInteract)
        {
            PushE();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            playerInteract = other.GetComponent<IInteractNpc>();
            isInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            playerInteract = null;
            isInteract = false;
        }
    }
    public void PushE()
    {
        if (playerInteract != null && Input.GetKeyDown(KeyCode.E))
        {
            isInteract = false;
            Debug.Log("플레이어 E 키누르기 ");

            playerInteract.InteractNpc();
            Controller_Physics.SwitchCameraLock(true); //플레이어 움직임 및 카메라 정지
            //test.SetActive(false);
        }
    }
}
