using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarwhalNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        //»Ô°í·¡ 25
        myDialogue.CheckStateDialogue(25, state);
    }
}

