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
        mode = GunMode.Grab;
        rangeLimit = state.GrabRange;
        //myLayer = 1 << LayerMask.NameToLayer("MovedObject");
    }
    public int GrabShot { get { return -ammo; } set { ammo = -value; } }

    GameObject targetObj = null;
    Rigidbody targetRigid = null;           
    Vector3 followPos;

    float rangeLimit = 30f;
    float maxSpeed = 3f;
    public override void ShootGun()
    {
        if(CheckCanFire() == false)
        {
            return;
        }

        OneShotGrab();
    }

    void OneShotGrab()
    {        
        if (targetRigid || state.Ammo < -ammo)
        {
            CancelObj();
            return;
        }
                
        if (state.hit.transform == null)
        {
            return;
        }

        state.pickupPoint.position = state.hit.point;

        float distanceCheck = Vector3.Distance(state.startPoint, state.pickupPoint.position);
        float distanceCheck2 = Vector3.Distance(state.startPoint, state.hit.transform.position);

        if (distanceCheck <= rangeLimit)
        {
            Debug.Log("못드는 거리 : " + distanceCheck);
            return;
        }

        FollowingObj();
        
    }

    //private void Update()
    //{
    //    if (targetRigid)
    //    {
    //        //float distanceCheck = Vector3.Distance(state.startPoint, targetObj.transform.position);
    //        float distanceCheck2 = Vector3.Distance(state.checkPos.position, state.pickupPoint.position);

    //        //Debug.Log("오브젝트 피벗 거리" + distanceCheck);
    //        Debug.Log("픽업 포인트 거리" + distanceCheck2);
    //    }

    //}

    private void FixedUpdate()
    {
        GrabObj();
    }

    public void GrabObj()
    {
        if (targetRigid)
        {
            Debug.Log(targetRigid.name);
            Ray checkRAy = new Ray(state.checkPos.position, -(state.checkPos.up * 10f));
            RaycastHit hit;


            float wheel = Input.GetAxis("Mouse ScrollWheel");

            float distanceCheck = Vector3.Distance(state.checkPos.position, targetObj.transform.position);
            float distanceCheck2 = Vector3.Distance(state.checkPos.position, state.pickupPoint.position);            

            if (wheel < 0f)
            {
                PulledObj(wheel);
            }

            Vector3 dir = (state.pickupPoint.position - followPos) - targetRigid.position;
            float mag = dir.magnitude;

            if (mag >= maxSpeed)
            {
                mag = maxSpeed;
            }

            state.grabLine.enabled = true;
            state.grabLine.SetPosition(0, state.GunHolderHand.position);
            state.grabLine.SetPosition(1, state.pickupPoint.position);

            targetRigid.velocity = dir * mag;

            if (Physics.Raycast(checkRAy, out hit, 20f, myLayer))
            {
                CancelObj();
            }

            //Debug.Log("픽업 포인트 위치는?" + state.pickupPoint.position);

            //if(distanceCheck2 < rangeLimit + 2f)
            //{
            //    //Debug.Log("너무 가까움");
            //    //Vector3 targetDir = state.pickupPoint.localPosition - state.checkPos.localPosition;
            //    //state.pickupPoint.localPosition += targetDir.normalized;
            //    //targetObj.transform.position += targetDir.normalized;
            //}

            //if (false
            //    //||distanceCheck <= rangeLimit
            //    || distanceCheck2 <= rangeLimit)
            //{
            //    CancelObj();
            //}

            if (false
                ||distanceCheck <= rangeLimit
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

        targetObj.GetComponent<Collider>().material.dynamicFriction = 1f;
        targetObj = null;
        state.grabLine.enabled = false;
        state.onGrab = false;        
    }

    public void CancleGrab()
    {
        targetObj.GetComponent<Collider>().material.dynamicFriction = 1f;
        targetObj = null;
        state.grabLine.enabled = false;
        targetRigid = null;
        state.onGrab = false;        
    }

    void FollowingObj( )
    {
        state.onGrab = true;
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
            //state.UpdateState(ammo);
            UsedAmmo(ammo);
        }
    }

    void PulledObj(float wheelData)
    {
        Vector3 dir = state.checkPos.localPosition - state.pickupPoint.localPosition;

        if (Vector3.Distance(state.pickupPoint.position, state.startPoint) < rangeLimit + 5f)
        {
            return;
        }

        dir = dir.normalized * (wheelData * 2f);
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
