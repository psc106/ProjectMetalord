using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    PlayerValue playerValue;

    private void OnCollisionEnter(Collision collision)
    {
        //추후 맞는 레이어로 변경
        if(collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (!playerValue.isGround)
            {
                playerValue.SetGroundState(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //추후 맞는 레이어로 변경
        //아이템 상호작용
        if (other.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            IInteractObject interactObject = other.GetComponent<IInteractObject>();

            //상호작용 가능 상태일 경우 아이템에 popup
            if (interactObject != null)
            {
                if (playerValue.interactObject == null)
                {
                    playerValue.interactObject = other.gameObject;
                }
                else if (CompareClosedDistance(playerValue.interactObject.transform.position,other.transform.position))
                {
                    playerValue.interactObject = other.gameObject;
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //추후 맞는 레이어로 변경
        //아이템 상호작용
        if (other.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            IInteractObject interactObject = other.GetComponent<IInteractObject>();

            //상호작용 가능 상태일 경우 아이템에 popup
            if (interactObject != null)
            {
                if (playerValue.interactObject == other.gameObject)
                {
                    playerValue.interactObject = null;
                }
            }
        }

    }

    bool CompareClosedDistance(Vector3 curr, Vector3 before)
    {
        return (Vector3.Distance(curr, transform.position) <= Vector3.Distance(before, transform.position));
    }
}
