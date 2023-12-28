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
        //state.onGrab = true;

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
        Debug.Log("여기에 도달하는건가?");
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

    void FollowingObj( )
    {
        state.onGrab = true;
        targetObj = state.hit.transform.gameObject;
        targetObj.GetComponent<SSC_BondObj>().Invoke("StateChagned", 2f);
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
