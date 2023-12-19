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

    private void Update()
    {
        if(isGrabed == true)
        {
            if(myRigid != null && myRigid.IsSleeping())
            {
                Destroy(myRigid);
                myColid.convex = false;
                isGrabed = false;                
            }
        }
    }

    public void ChangedState()
    {        
        myRigid = GetComponent<Rigidbody>();
        isGrabed = true;
    }

}
