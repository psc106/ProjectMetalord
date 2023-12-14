using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SSC_GrabObj : MonoBehaviour
{
    bool isGrabed = false;
    Rigidbody myRigid;
    MeshCollider myColid;

    private void Awake()
    {        
        myColid = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        if(isGrabed == true)
        {
            if(myRigid != null && myRigid.IsSleeping())
            {
                myColid.convex = false;
                Destroy(myRigid);
                isGrabed = false;
            }
        }
    }

    public void ChangedState(bool _ChangedRigid)
    {        
        myRigid = GetComponent<Rigidbody>();
        myColid.convex = true;
        myRigid.useGravity = false;
        myRigid.constraints = RigidbodyConstraints.FreezeRotation;
        isGrabed = true;

        if(myRigid.isKinematic == true)
        {
            myRigid.isKinematic = _ChangedRigid;
        }

    }
    
}
