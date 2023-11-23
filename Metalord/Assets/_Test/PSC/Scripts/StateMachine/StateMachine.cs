using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, StateBase<EState>> state = new Dictionary<EState, StateBase<EState>>();

    protected StateBase<EState> currState;

    protected bool isTransitioningState = false;

    protected virtual void Start()
    {
        currState.Enter();
        
    }

    protected virtual void FixedUpdate()
    {

        currState.FixedUpdateFunc();

    }
    protected virtual void Update()
    {

        currState.UpdateFunc();
        /* EState nextKey = currState.GetNextState();
         if (!isTransitioningState && nextKey.Equals(currState.stateKey))
         {
             currState.UpdateFunc();
         }
         else
         {
             TransitionToState(nextKey);
         }*/

    }
    protected virtual void LateUpdate()
    {

        currState.LateUpdateFunc();

    }

    void TransitionToState(EState nextState)
    {
        isTransitioningState = true;
        currState.Exit();
        currState = state[nextState];
        currState.Enter();
        isTransitioningState = false;
    }

}

