using System;
using Unity.VisualScripting;
using UnityEngine;

[Obsolete]
public class Legacy_PlayerCollision : MonoBehaviour
{

    [SerializeField]
    Legacy_PlayerValue playerValue;

    private void OnTriggerStay(Collider other)
    {

        if(playerValue.playerState == PlayerStateName.GRAB || (playerValue.interactObject != null && other.gameObject == playerValue.interactObject.gameObject))
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("InteractObject"))
        {
            IInteractObject interactObject = other.GetComponent<IInteractObject>();

            //상호작용 가능 상태일 경우 아이템에 popup

            if (interactObject != null)
            {
                if (playerValue.interactObject == null) {

                    other.GetComponent<Renderer>().material.color = Color.blue;
                    playerValue.interactObject = other.gameObject.GetComponent<ItemBaseTest>();

                }
                else if (CompareClosedDistance(other.transform.position,playerValue.interactObject.transform.position))
                {
                    playerValue.interactObject.GetComponent<Renderer>().material.color = Color.gray;

                    other.GetComponent<Renderer>().material.color = Color.blue;
                    playerValue.interactObject = other.gameObject.GetComponent<ItemBaseTest>();
                }

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (playerValue.playerState == PlayerStateName.GRAB)
        {
            return;
        }

        //추후 맞는 레이어로 변경
        //아이템 상호작용
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractObject"))
        {
            IInteractObject interactObject = other.GetComponent<IInteractObject>();

            //상호작용 가능 상태일 경우 아이템에 popup
            if (interactObject != null)
            {
                Debug.Log("out : "+playerValue.interactObject+"/"+ other.gameObject);
                if (playerValue.interactObject.gameObject == other.gameObject)
                {
                    playerValue.interactObject.GetComponent<Renderer>().material.color = Color.gray;
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
