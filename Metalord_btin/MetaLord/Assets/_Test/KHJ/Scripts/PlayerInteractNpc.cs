using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if(other.gameObject.layer == LayerMask.NameToLayer("InteractZone"))
        {
            Debug.Log("찍히나?");
            playerInteract = other.transform.parent.GetComponent<IInteractNpc>();
                //other.GetComponent<IInteractNpc>();
            isInteract = true;
            Debug.Log(playerInteract);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractZone"))
        {
            Debug.Log("나오나?");
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
