
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
        myDialogue.CheckStateTutorialDialogue(state);
    }

}
