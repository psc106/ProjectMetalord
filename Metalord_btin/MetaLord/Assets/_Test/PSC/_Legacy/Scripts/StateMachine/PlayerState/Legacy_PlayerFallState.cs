using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class Legacy_PlayerFallState : Legacy_StateBase<PlayerStateName>
{
    public Legacy_PlayerFallState(PlayerStateName key) : base(key)
    {
    }
    public Legacy_PlayerFallState(PlayerStateName key, Legacy_PlayerValue playerValue) : base(key)
    {
    }


    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdateFunc()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateFunc()
    {
        throw new System.NotImplementedException();
    }

    public override void LateUpdateFunc()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override PlayerStateName GetNextState()
    {
        throw new System.NotImplementedException();
    }
}
