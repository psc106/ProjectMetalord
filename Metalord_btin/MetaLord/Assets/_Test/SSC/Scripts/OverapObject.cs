using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverapObject : MonoBehaviour
{
    Rigidbody myRigid;
    MeshCollider myColid;
    OverapObject myScript;
    MovedObject myMoved;

    public void InitOverap(GunStateController _state)
    {
        if (myScript == null)
        {
            myScript = this;
            myScript = GetComponent<OverapObject>();
            myMoved  = GetComponent<MovedObject>();
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

        myMoved.CareeState(_state, myRigid);
             
    }
}
