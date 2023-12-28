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
    
    public virtual void ChangedState(npcState _change)
    {
            Debug.LogFormat("{0} <==현재 변경될 상태, {1}<====현재 상태", _change, state);
        if(_change == npcState.objectAttached)
        {
            Debug.LogFormat("변경될 상태가 오브젝트 붙은 상태일떄 실행되는 메서드 {0} <==현재 변경될 상태, {1}<====현재 상태", _change, state);

            if (state == npcState.glued || state == npcState.objectAttached)
            { 
                state = _change;
            }
            else
            {
                return;
            }
        }
        else if (_change == npcState.glued)
        {
            Debug.LogFormat("본드 상태일때 들어오는 상태{0} <==현재 변경될 상태, {1}<====현재 상태", _change, state);

            if (state == npcState.objectAttached)
            {
                state = npcState.objectAttached;
            }
            else
            {
                state = _change;
            }
        }
        else
        {
            state = _change;
        }
    }

    public void PrintState()
    {
        Debug.LogFormat("{0} <== {1} 상태",state, transform.name);
    }
}
