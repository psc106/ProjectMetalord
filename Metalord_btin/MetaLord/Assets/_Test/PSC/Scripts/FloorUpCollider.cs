using System.Collections;
using UnityEngine;

public class FloorUpCollider : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;


    private void OnTriggerEnter(Collider other)
    {

        Rigidbody rb = other.GetComponent <Rigidbody>();

        if (rb)
        {
            other.excludeLayers = layerMask;

            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up* 20, ForceMode.VelocityChange);
            StartCoroutine(ResetLayerRoutine(other));
        }
    }

    IEnumerator ResetLayerRoutine(Collider other)
    {
        yield return new WaitForSeconds(1);
        other.excludeLayers = 0;
    }
}
