using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SSC_GrabObj : MonoBehaviour
{
    bool isGrabed = false;
    Rigidbody myRigid;
    MeshCollider childColid;

    private void Awake()
    {
        //myRigid = GetComponent<Rigidbody>();
        childColid = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        if(isGrabed == true)
        {
            if(myRigid != null && myRigid.IsSleeping())
            {
                childColid.convex = false;
                Destroy(myRigid);
                isGrabed = false;
            }
        }
    }

    public void ChangedState(bool _ChangedRigid)
    {        
        myRigid = GetComponent<Rigidbody>();
        myRigid.useGravity = false;
        isGrabed = true;

        if(myRigid.isKinematic == true)
        {
            myRigid.isKinematic = _ChangedRigid;
        }

    }
}
