using Unity.VisualScripting;
using UnityEngine;

public class SSC_BondObj : MonoBehaviour
{
    SSC_GrabGun grab;
    MeshCollider myColider;        
    public LayerMask layerMask;

    private void Awake()
    {
        grab = FindAnyObjectByType<SSC_GrabGun>();       
        myColider = GetComponent<MeshCollider>();
    }

    private void OnCollisionStay(Collision collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 dir = collision.contacts[i].normal;

            Ray ray = new Ray(collision.contacts[i].point + dir, -dir);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1.5f, layerMask) &&
                PaintTarget.RayColor(ray) == Color.red)
            {
                transform.parent = hit.transform;
                SSC_GunState.AddBondList(this);

                if (transform.GetComponent<Rigidbody>() != null)
                {
                    Destroy(transform.GetComponent<Rigidbody>());
                    myColider.convex = false;
                    grab.CancleGrab();
                }
            }            
        }
    }

    private void FixedUpdate()
    {
        if(transform.GetComponent<Rigidbody>() != null)
        {
            if(transform.GetComponent<Rigidbody>().IsSleeping())
            {
                Destroy(transform.GetComponent<Rigidbody>());
                myColider.convex = false;
            }
        }
    }

    public void CelarBond()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
            myColider.convex = true;

            transform.AddComponent<Rigidbody>().useGravity = true;
        }
    }
}
