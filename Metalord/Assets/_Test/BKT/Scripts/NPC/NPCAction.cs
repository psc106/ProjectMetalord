using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAction
{
    public NPCInfo info;

    public NPCState state;
    private int currentActionStepIndex;

    public NPCAction(NPCInfo _npcInfo)
    {
        this.info = _npcInfo;
        this.state = NPCState.REQUIREMENTS_NOT_MET;
        this.currentActionStepIndex = 0;
    }

    public NPCAction(NPCInfo _npcInfo, NPCState _npcState, int _currentActionStepIndex)
    {
        this.info = _npcInfo;
        this.state = _npcState;
        this.currentActionStepIndex = _currentActionStepIndex;
    }

    public void MoveToNextStep()
    {
        currentActionStepIndex++;
    }
}
