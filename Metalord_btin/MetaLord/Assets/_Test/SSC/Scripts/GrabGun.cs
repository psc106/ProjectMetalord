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
        //myLayer = 1 << LayerMask.NameToLayer("MovedObject");
    }

    public int GrabShot { get { return -ammo; } set { ammo = -value; } }

    GameObject targetObj = null;
    Rigidbody targetRigid = null;           
    Vector3 followPos;
    
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
        if (targetRigid || state.Ammo < - ammo)
        {            
            CancelObj();
            return;
        }
                
        if (state.hit.transform == null)
        {
            return;
        }

        state.pickupPoint.position = state.hit.point;

        //float distanceCheck = Vector3.Distance(state.startPoint, state.pickupPoint.position);
        //float distanceCheck2 = Vector3.Distance(state.startPoint, state.hit.transform.position);

        //if (distanceCheck <= rangeLimit ||
        //    distanceCheck2 <= rangeLimit)
        //{
        //    Debug.Log("못드는 거리1 : " + distanceCheck);
        //    Debug.Log("못드는 거리2 : " + distanceCheck2);
        //    return;
        //}

        FollowingObj();        
    }

    private void Update()
    {
        GrabObj();
    }

    public void GrabObj()
    {
        if (targetRigid)
        {
            if (state.getconnect() == targetRigid)
            {
                CancelObj();
                return;
            }    

            // LEGACY : 휠 다운 오브젝트 당기는 기능 삭제
            //float wheel = Input.GetAxis("Mouse ScrollWheel");
            //if (wheel < 0f)
            //{
            //    PulledObj(wheel);
            //}

            //float distanceCheck = Vector3.Distance(state.checkPos.position, targetObj.transform.position);
            //float distanceCheck2 = Vector3.Distance(state.checkPos.position, state.pickupPoint.position);

            //Debug.Log("트랜스폼" + distanceCheck);            
            //Debug.Log("픽업" + distanceCheck2);


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
            targetObj.GetComponent<Collider>().material.dynamicFriction = 1f;
        }

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

    // LEGACY : 휠 다운 오브젝트 당기는 기능 삭제
    //void PulledObj(float wheelData)
    //{
    //    Vector3 dir = state.checkPos.localPosition - state.pickupPoint.localPosition;

    //    if (Vector3.Distance(state.pickupPoint.position, state.startPoint) < rangeLimit + 5f)
    //    {
    //        return;
    //    }

    //    dir = dir.normalized * (wheelData * 2f);
    //    state.pickupPoint.transform.localPosition -= dir;
    //}

    protected override bool CheckCanFire()
    {
        Vector3 start = state.checkPos.position;
        start.y -= 2f;
        Ray checkRay = new Ray(start, -(state.checkPos.up));
        RaycastHit hit;
        Physics.SphereCast(checkRay, 2f, out hit, 20f, myLayer);        

        // 플레이어 밑에 있는 오브젝트가 내 조준점에 담긴 오브젝트라면 그랩시도 X
        if (!state.CanFire || hit.transform?.gameObject == state.hit.transform?.gameObject)
        {            
            return false;        
        }

        return true;
    }
}
