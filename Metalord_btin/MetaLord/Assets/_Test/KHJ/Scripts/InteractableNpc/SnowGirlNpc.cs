using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGirlNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        // ´«»ç¶÷ 28
        myDialogue.CheckStateDialogue(28, state);
    }
}