using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.ShowDialogue(12);
    }
}
