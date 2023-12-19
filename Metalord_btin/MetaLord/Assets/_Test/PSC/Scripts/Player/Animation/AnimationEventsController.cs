using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsController : MonoBehaviour
{
    [SerializeField]
    Controller_Physics player;

    UnityAction reloadEnd;
    UnityAction unequipEnd;
    UnityAction equipEnd;

    UnityAction[] animationEvents;

    private void Start()
    {
        animationEvents = new UnityAction[3];
        animationEvents[0] = reloadEnd;
        animationEvents[1] = unequipEnd;
        animationEvents[2] = equipEnd;
    }

    private void OnEnable()
    {
        reloadEnd += player.EndReloadAnimation;
        unequipEnd += player.EndUnEquipAnimation;
        equipEnd += player.EndEquipAnimation;
    }

    private void OnDisable()
    {
        reloadEnd -= player.EndReloadAnimation;
        unequipEnd -= player.EndUnEquipAnimation;
        equipEnd -= player.EndEquipAnimation;
    }

    public void PlayAnimation(AnimationList listNum)
    {
        animationEvents[(int)listNum].Invoke();
    }

    public enum AnimationList
    {
        reloadEnd=0, unequipEnd = 1, equipEnd = 2
    }
}
