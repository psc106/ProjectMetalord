using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum npcState
{
    normal,
    glued,
    objectAttached
}

public class NpcBase : MonoBehaviour
{

    protected npcState state;

    [SerializeField] protected DialogueUI myDialogue;

    protected virtual void Awake()
    {
        myDialogue = GameObject.Find("DialogueCanvas").GetComponent<DialogueUI>();
        state = npcState.normal;                
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.H))
    //    {
    //        ChangedState(npcState.normal);
    //        Debug.Log("기본 상태");
    //    }

    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        ChangedState(npcState.glued);
    //        Debug.Log("접착제 묻은 상태");
    //    }

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        ChangedState(npcState.objectAttached);
    //        Debug.Log("물건 붙은 상태");
    //    }
    //}
    
    public void ChangedState(npcState _change)
    {
        state = _change;
    }

    public void PrintState()
    {
        Debug.LogFormat("{0} <== {1} 상태",state, transform.name);
    }
}
