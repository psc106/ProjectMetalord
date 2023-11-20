using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public GameObject TEST;

    [SerializeField]
    PlayerValue playerValue;
    private void OnCollisionEnter(Collision collision)
    {
        //추후 맞는 레이어로 변경
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!playerValue.CheckGround)
            {
                playerValue.CheckGround = (true);
                playerValue.playerState = PlayerState.IDLE;
                playerValue.extraGravity.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("InteractObject"))
        {
            IInteractObject interactObject = other.GetComponent<IInteractObject>();

            //상호작용 가능 상태일 경우 아이템에 popup

            if (interactObject != null)
            {
                Debug.Log(playerValue.interactObject);
                if (playerValue.playerState != PlayerState.GRAB)
                {
                    playerValue.interactObject = other.gameObject.GetComponent<ItemBaseTest>();
                }
                else if (CompareClosedDistance(playerValue.interactObject.transform.position, other.transform.position))
                {
                    playerValue.interactObject = other.gameObject.GetComponent<ItemBaseTest>();
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (playerValue.playerState == PlayerState.GRAB)
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
