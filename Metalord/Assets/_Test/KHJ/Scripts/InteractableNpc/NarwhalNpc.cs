using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarwhalNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.ShowDialogue(16);
    }
}

