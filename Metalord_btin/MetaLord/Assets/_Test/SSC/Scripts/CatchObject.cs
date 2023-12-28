using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObject : MonoBehaviour
{
    Rigidbody myRigid;
    List<MeshCollider> childColid = new List<MeshCollider>();

    public void InitCatch()
    {
        if(myRigid == null)
        {
            myRigid = GetComponent<Rigidbody>();
            myRigid.mass = 1000f;
        }

        childColid.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {            
            childColid.Add(transform.GetChild(i).GetComponent<MeshCollider>());            
        }
    }

    public void AddChild(MeshCollider _child)
    {
        childColid.Add(_child);        
    }

    private void Update()
    {
        if(myRigid)
        {
            if(myRigid.IsSleeping())
            {
                Destroy(myRigid);

                for(int i = 0; i < childColid.Count; i++)
                {
                    childColid[i].convex = false;
                }
            }
        }
    }
}
