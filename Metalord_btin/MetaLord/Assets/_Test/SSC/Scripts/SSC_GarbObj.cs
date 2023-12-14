using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_GarbObj : MonoBehaviour
{
    bool isGrabed = false;
    Rigidbody myRigid;
    public MeshCollider childColid;

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody>();        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(isGrabed)
        //{
        //    return;
        //}

        //childColid.enabled = true;
    }

    public void ChangedState(bool _ChangedRigid)
    {
        //isGrabed = true;

        if(myRigid.isKinematic == true)
        {
            myRigid.isKinematic = _ChangedRigid;
        }

    }
}
