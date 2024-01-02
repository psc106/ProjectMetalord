using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverapObject : MonoBehaviour
{
    Rigidbody myRigid;
    MeshCollider myColid;
    OverapObject myScript;

    public void InitOverap()
    {
        if (myScript == null)
        { 
            myScript = GetComponent<OverapObject>();
        }

        if(myRigid == null)
        {
            transform.gameObject.AddComponent<Rigidbody>();
            myRigid = transform.GetComponent<Rigidbody>();
            myRigid.mass = 1000f;
            myRigid.AddForce(Vector3.down * 100f, ForceMode.Force);
        }

        if(myColid == null)
        {
            myColid = GetComponent<MeshCollider>();
            myColid.material.dynamicFriction = 1f;
            myColid.convex = true;

        }
    }

    private void Update()
    {
        if (myRigid)
        {
            if (myRigid.IsSleeping())
            {                
                Destroy(myRigid);
                myColid.convex = false;
                myColid.material.dynamicFriction = 0.1f;
                Destroy(myScript);
            }
        }
    }


}
