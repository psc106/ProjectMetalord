using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    [SerializeField] protected DialogueUI myDialogue;

    protected virtual void Awake()
    {
        myDialogue = GameObject.Find("TutorialDialogueCanvas").GetComponent<DialogueUI>();
    }
    
}
