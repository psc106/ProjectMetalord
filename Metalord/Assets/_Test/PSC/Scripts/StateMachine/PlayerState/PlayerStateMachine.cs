using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerStateName>
{

    public PlayerValue playerValue;

    private void Awake()
    {
        state.Add(PlayerStateName.IDLE, new PlayerIdleState(PlayerStateName.IDLE, playerValue));
        state.Add(PlayerStateName.GRAB, new PlayerIdleState(PlayerStateName.GRAB, playerValue));
        state.Add(PlayerStateName.FALL, new PlayerIdleState(PlayerStateName.FALL, playerValue));
        state.Add(PlayerStateName.JUMP, new PlayerIdleState(PlayerStateName.JUMP, playerValue));
        state.Add(PlayerStateName.UMBRELLA, new PlayerIdleState(PlayerStateName.UMBRELLA, playerValue));

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