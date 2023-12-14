using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        Debug.Log("말 스크립트 실행 안되는건가?");
        myDialogue.CheckStateDialogue(25, state);
    }
}

