using Unity.VisualScripting;
using UnityEngine;

public class SSC_BondObj : MonoBehaviour
{
    SSC_GrabGun grab;
    MeshCollider myColider;        
    public LayerMask layerMask;

    [HideInInspector] public Rigidbody myRigid;

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

    void FixedUpdate()
    {
        if(myRigid)
        {
            if(myRigid.IsSleeping())
            {
                Destroy(myRigid);
                myRigid = null;
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

            myRigid = transform.AddComponent<Rigidbody>();
            myRigid.useGravity = true;
            //transform.AddComponent<Rigidbody>().useGravity = true;
        }
    }
}
