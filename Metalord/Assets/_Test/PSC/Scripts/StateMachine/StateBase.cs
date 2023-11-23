using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase<EState> where EState : Enum
{

    public StateBase(EState key)
    {
        stateKey = key;
    }
    public EState stateKey { get; private set; }
    public abstract void Enter();
    public abstract void FixedUpdateFunc();
    public abstract void UpdateFunc();
    public abstract void LateUpdateFunc();

    public abstract void Exit();

    public abstract EState GetNextState();

}
