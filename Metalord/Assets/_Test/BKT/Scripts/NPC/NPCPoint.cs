using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPoint : MonoBehaviour
{
    [SerializeField] private NPCInfo npcInfoForPoint;

    [SerializeField] private bool isNPCPoint = true;

    private bool playerIsNear = false;
    private string npcId;
    private NPCState currentNPCState;

    private NPCIcon npcIcon;

    private void Awake()
    {
        npcId = npcInfoForPoint.id;
        npcIcon = GetComponentInChildren<NPCIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.npcEvents.onNPCStateChange += NPCStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCStateChange -= NPCStateChange;

    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        if(currentNPCState.Equals(NPCState.CAN_START))
        {
            GameEventsManager.instance.npcEvents.StartNPCAction(npcId);
        }
        else if (currentNPCState.Equals(NPCState.FINISHED))
        {
            GameEventsManager.instance.npcEvents.FinishNPCAction(npcId);
        }
    }

    private void NPCStateChange(NPCAction npcAction)
    {
        if (npcAction.info.id.Equals(npcId))
        {
            currentNPCState = npcAction.state;
            npcIcon.SetState(currentNPCState, isNPCPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }


}
