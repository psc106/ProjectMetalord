using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNpc : MonoBehaviour, IInteractNpc
{
    public DialogueUI myDialogue;

    public void InteractNpc()
    {
        Debug.Log("Interact 실행됨");
        myDialogue.ShowTutoDialogue(1);
    }

}
