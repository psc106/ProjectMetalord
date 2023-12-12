using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeNpc : NpcBase, IInteractNpc
{
    //꿀벌 : 16
    public void InteractNpc()
    {
        myDialogue.CheckStateDialogue(16,state);
    }
}
