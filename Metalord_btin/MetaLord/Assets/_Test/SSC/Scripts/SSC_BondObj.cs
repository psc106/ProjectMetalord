using Unity.VisualScripting;
using UnityEngine;

public class SSC_BondObj : MonoBehaviour
{    
    MeshCollider myColider;        
    public LayerMask layerMask;

    [HideInInspector] public Rigidbody myRigid;

    private void Awake()
    {            
        myColider = GetComponent<MeshCollider>();

        layerMask = 1 << LayerMask.NameToLayer("Default") |
            1 << LayerMask.NameToLayer("NPC") |
            1 << LayerMask.NameToLayer("StaticObject") |
            1 << LayerMask.NameToLayer("MovedObject");
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
                if(hit.transform.GetComponent<NpcBase>() != null)
                {
                    hit.transform.GetComponent<NpcBase>().ChangedState(npcState.objectAttached);
                    GunStateController.AddList(hit.transform.GetComponent<NpcBase>());
                }

                transform.parent = hit.transform;
                GunStateController.AddList(this);

                if (transform.GetComponent<Rigidbody>() != null)
                {
                    Destroy(transform.GetComponent<Rigidbody>());
                    myColider.convex = false;
                    GrabGun.instance.CancelObj();                    
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
            myRigid.mass = 1000f;
            myRigid.useGravity = true;            
        }
    }
}
