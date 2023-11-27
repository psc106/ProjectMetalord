using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents
{
    public event Action<string> onStartNPCAction;
    public void StartNPCAction(string id)
    {
        if (onStartNPCAction != null)
        {
            onStartNPCAction(id);
        }
    }

    public event Action<string> onAdvanceNPCAction;
    public void AdvanceNPCAction(string id)
    {
        if (onAdvanceNPCAction != null)
        {
            onAdvanceNPCAction(id);
        }
    }

    public event Action<string> onFinishNPCAction;
    public void FinishNPCAction(string id)
    {
        if (onFinishNPCAction != null)
        {
            onFinishNPCAction(id);
        }
    }

    public event Action<NPCAction> onNPCStateChange;
    public void NPCStateChange(NPCAction npcAction)
    {
        if (onNPCStateChange != null)
        {
            onNPCStateChange(npcAction);
        }
    }
}
