using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.ShowDialogue(15);
    }
}

