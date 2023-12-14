using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActions
{
    Move,
    Jump,
    Fire,
    Pull,
    Run,
}

public enum BindingKeyword
{
    
}

public class RebindKeyUI : MonoBehaviour
{
    public InputReader inputReader;
    public InputAction gameInputAction;

    //public PlayerActions playerActions = PlayerActions.Forward;

    private void Awake()
    {
        gameInputAction = inputReader.inputActions.Player.Move;

        //switch (playerActions)
        //{
        //    case PlayerActions.Forward:
        //        gameInputAction.PerformInteractiveRebinding()
        //            .WithTargetBinding(2)
        //            .Start();
                    
        //        break;
        //}
    }

    private int FindTargetBinding()
    {



        return 0;
    }

    public void onClickMoveForward()
    {
        gameInputAction.Disable();

        Debug.Log("입력 들어옴");
        gameInputAction.PerformInteractiveRebinding()
                    .WithTargetBinding(2)
                    .OnComplete(a=>gameInputAction.Enable())
                    .Start();
    }
}
