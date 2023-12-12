using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class Legacy_PlayerStateMachine : Legacy_StateMachine<PlayerStateName>
{

    public Legacy_PlayerValue playerValue;

    private void Awake()
    {
        state.Add(PlayerStateName.IDLE, new Legacy_PlayerIdleState(PlayerStateName.IDLE, playerValue));
        state.Add(PlayerStateName.GRAB, new Legacy_PlayerIdleState(PlayerStateName.GRAB, playerValue));
        state.Add(PlayerStateName.FALL, new Legacy_PlayerIdleState(PlayerStateName.FALL, playerValue));
        state.Add(PlayerStateName.JUMP, new Legacy_PlayerIdleState(PlayerStateName.JUMP, playerValue));
        state.Add(PlayerStateName.UMBRELLA, new Legacy_PlayerIdleState(PlayerStateName.UMBRELLA, playerValue));

        currState = state[PlayerStateName.IDLE];
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void LateUpdate()
    {

        base.LateUpdate();

    }
}

public enum PlayerStateName
{
    IDLE = 0, GRAB, FALL, JUMP, UMBRELLA, SYSTEM_STOP
}