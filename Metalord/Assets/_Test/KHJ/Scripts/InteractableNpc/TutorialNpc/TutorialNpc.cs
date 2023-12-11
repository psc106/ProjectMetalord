using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNpc : NpcBase, IInteractNpc
{
    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    public void InteractNpc()
    {
        Debug.Log("Interact 실행됨");
        myDialogue.ShowTutoDialogue(1);
    }

}
