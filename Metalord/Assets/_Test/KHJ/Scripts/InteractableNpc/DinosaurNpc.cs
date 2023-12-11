using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.ShowDialogue(13);
    }
}

