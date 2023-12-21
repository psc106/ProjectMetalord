using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabGun : GunBase
{
    public static GrabGun instance;    
    protected override void Awake()
    {
        instance = this;
        base.Awake();
        brush.splatChannel = 2;
        ammo = -55;

        //myLayer = 1 << LayerMask.NameToLayer("MovedObject");
    }

    GameObject targetObj = null;
    Rigidbody targetRigid = null;                 
    Vector3 followPos;

    float rangeLimit = 20f;    
    public override void ShootGun()
    {
        if(CheckCanFire() == false)
        {
            return;
        }

        // TODO : state.hit에 정보가 안담겼을 때 Null값을 참조하지 않게 만들어야 함

        OneShotGrab();
    }

    void OneShotGrab()
    {        
        if (targetRigid || state.Ammo < -ammo)
        {
            CancelObj();
            return;
        }       

        if(state.hit.transform != null)
        {
            state.pickupPoint.position = state.hit.point;
        }
        float distanceCheck = Vector3.Distance(state.startPoint, state.pickupPoint.position);
        //float distanceCheck2 = Vector3.Distance(state.startPoint, state.hit.transform.position);

        if (distanceCheck <= rangeLimit)
        {
            return;
        }

        OnGrab = true;
        FollowingObj();
        
    }

    private void FixedUpdate()
    {
        GrabObj();
    }

    public void GrabObj()
    {
        if (targetRigid)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");

            if (wheel < 0f)
            {
                PulledObj(wheel);
            }

            Vector3 dir = (state.pickupPoint.position - followPos) - targetRigid.position;
            float mag = dir.magnitude;

            if (mag >= 2f)
            {
                mag = 2f;
            }

            state.grabLine.enabled = true;
            state.grabLine.SetPosition(0, state.GunHolderHand.position);
            state.grabLine.SetPosition(1, state.pickupPoint.position);

            targetRigid.velocity = dir * mag;

            float distanceCheck = Vector3.Distance(state.startPoint, targetObj.transform.position);
            float distanceCheck2 = Vector3.Distance(state.startPoint, state.pickupPoint.position);

            if (distanceCheck <= rangeLimit
                || distanceCheck2 <= rangeLimit)
            {
                CancelObj();
            }
            
        }
    }


    public void CancelObj()
    {
        if(targetRigid != null)
        {
            targetRigid.constraints = RigidbodyConstraints.None;
            targetRigid.useGravity = true;
            targetRigid.velocity = Vector3.zero;
            targetRigid = null;
        }

        targetObj = null;
        state.grabLine.enabled = false;
        OnGrab = false;        
    }

    public void CancleGrab()
    {
        targetObj = null;
        state.grabLine.enabled = false;
        targetRigid = null;
        OnGrab = false;        
    }

    void FollowingObj( )
    {        
        targetObj = state.hit.transform.gameObject;
        state.pickupPoint.position = state.hit.point;
        followPos = state.pickupPoint.position - state.hit.transform.position;
        state.hit.transform.GetComponent<MeshCollider>().convex = true;
        state.hit.transform.AddComponent<Rigidbody>();
        targetRigid = state.hit.rigidbody;
        state.hit.transform.GetComponent<SSC_GrabObj>().ChangedState();
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;


        if(state.Ammo > -ammo)
        {
            state.UpdateState(ammo);
        }
    }

    void PulledObj(float wheelData)
    {
        Vector3 dir = state.checkPos.localPosition - state.pickupPoint.localPosition;

        if (Vector3.Distance(state.pickupPoint.position, state.startPoint) < 25f)
        {
            return;
        }

        dir = dir.normalized * wheelData;
        state.pickupPoint.transform.localPosition -= dir;
    }

    protected override bool CheckCanFire()
    {        
        if (!state.CanFire)
        {        
            return false;        
        }

        return true;
    }
}
