using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        // ·Îº¿ 31
        myDialogue.CheckStateDialogue(31, state);
    }
}