using Unity.VisualScripting;
using UnityEngine;

public class GrabObj : MonoBehaviour
{
    //bool isGrabed = false;
    //Rigidbody myRigid;
    //MeshCollider myColid;
    //GunStateController state;

    //private void Awake()
    //{        
    //    state = FindAnyObjectByType<GunStateController>();
    //    myColid = GetComponent<MeshCollider>();
    //}

    //private void Update()
    //{
    //    if(isGrabed == true)
    //    {
    //        if(myRigid != null && myRigid.IsSleeping())
    //        {
    //            Destroy(myRigid);
    //            myColid.convex = false;
    //            isGrabed = false;                
    //        }
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (myRigid)
    //    {
    //        float wheel = Input.GetAxis("Mouse ScrollWheel");

    //        if (wheel < 0f)
    //        {
    //            PulledObj(wheel);
    //        }

    //        Vector3 dir = (state.pickupPoint - followPos) - myRigid.position;
    //        float mag = dir.magnitude;

    //        if (mag >= 2f)
    //        {
    //            mag = 2f;
    //        }

    //        state.grabLine.enabled = true;
    //        state.grabLine.SetPosition(0, state.startPoint);
    //        state.grabLine.SetPosition(1, followObj.transform.position);

    //        myRigid.velocity = dir * mag;

    //        float distanceCheck = Vector3.Distance(state.startPoint, targetObj.transform.position);
    //        float distanceCheck2 = Vector3.Distance(state.startPoint, state.pickupPoint.position);

    //        if (distanceCheck <= rangeLimit
    //            || distanceCheck2 <= rangeLimit)
    //        {
    //            CancelObj();
    //        }
    //    }
    //}

    //public void ChangedState()
    //{        
    //    transform.AddComponent<Rigidbody>();
    //    myRigid = GetComponent<Rigidbody>();
    //    myRigid.useGravity = false;
    //    myRigid.constraints = RigidbodyConstraints.FreezeRotation;
    //    myColid.convex = true;
    //    isGrabed = true;
    //}

}
