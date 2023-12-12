using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImokidNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.CheckStateDialogue(22, state);
    }
}

