using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsController : MonoBehaviour
{
    [SerializeField]
    Controller_Physics player;

    UnityAction reloadStart;
    UnityAction reloadEnd;

    UnityAction[] animationEvents;

    private void Start()
    {
        animationEvents = new UnityAction[2];
        animationEvents[0] = reloadStart;
        animationEvents[1] = reloadEnd;
    }

    private void OnEnable()
    {
        reloadStart += player.PlayReloadAnimation;
        reloadEnd += player.EndReloadAnimation;
    }

    private void OnDisable()
    {
        reloadStart += player.PlayReloadAnimation;
        reloadEnd += player.EndReloadAnimation;
    }

    public void PlayAnimation(AnimationList listNum)
    {
        animationEvents[(int)listNum].Invoke();
    }

    public enum AnimationList
    {
        reloadStart=0, reloadEnd=1
    }
}
