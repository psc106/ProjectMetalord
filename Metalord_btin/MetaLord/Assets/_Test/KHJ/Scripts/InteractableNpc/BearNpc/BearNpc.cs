using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        // °õ 37
        myDialogue.CheckStateDialogue(37, state);
    }
}
