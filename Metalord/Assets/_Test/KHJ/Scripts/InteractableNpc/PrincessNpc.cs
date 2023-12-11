using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        myDialogue.ShowDialogue(17);
    }
}
