using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GrabGun : GunBase
{
    public static GrabGun instance;    
    protected override void Awake()
    {
        instance = this;
        base.Awake();
        brush.splatChannel = 2;
        //ammo = -55;
        mode = GunMode.Grab;
    }

    public int GrabShot { get { return -ammo; } set { ammo = -value; } }

    GameObject targetObj = null;
    Rigidbody targetRigid = null;           
    Vector3 followPos;
    
    float maxSpeed = 3f;
    public override bool ShootGun()
    {
        // 언락전 우클릭 입력시 UI 텍스트 출력
        if(!state.UsedGrabGun())
        {
            if(state.CanFire)
            {
                state.CheckUnlockUi();       
            }

            return false;
        }

        if(CheckCanFire() == false)
        {
            // onGrab 상태에서도 들고있는 물건 그랩 해제를 위해
            if (targetRigid || state.Ammo < -ammo)
            {
                CancelObj();
                return false;
            }

            state.CheckRangeCrossHair();
            return false;
        }

        return OneShotGrab();
    }

    bool OneShotGrab()
    {           
        if (targetRigid || state.Ammo < - ammo)
        {            
            CancelObj();
            return false;
        }

        if (state.hit.transform == null)
        {
            return false;
        }
        // TODO : 임시야매로 그랩건의 오브젝트 남발 방지
        else if (state.hit.transform.gameObject.layer != LayerMask.NameToLayer("MovedObject") &&
            state.hit.transform.gameObject.layer != LayerMask.NameToLayer("GrabedObject"))
        {            
            return false;
        }

        //state.pickupPoint.position = state.hit.point;
        //state.pickupPoint.position = state.hit.transform.position;

        FollowingObj();
        return true;
    }

    private void Update()
    {
        GrabObj();
    }

    public void GrabObj()
    {
        if (targetRigid)
        {
            if (state.GetConnectObject() == targetRigid)
            {
                CancelObj();
                return;
            }

            Vector3 dir = (state.pickupPoint.position - followPos) - targetRigid.position;
            //Vector3 dir = state.pickupPoint.position - targetRigid.position;
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

        }

        if(targetObj != null && targetObj.GetComponent<MeshCollider>() != null)
        {
            targetObj.GetComponent<MeshCollider>().material.dynamicFriction = 1f;
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
        state.isShootingState = false;
    }

    void FollowingObj( )
    {
        state.onGrab = true;
        targetObj = state.hit.transform.gameObject;        

        // 그랩 대상의 부모가 없다면
        if (targetObj.transform.parent == null)
        {
            // 공중에서 떨어지는 대상일 때 
            if(targetObj.transform.GetComponent<Rigidbody>() != null)
            {
                /* PASS */
            }
            else
            {                
                targetObj.GetComponent<MeshCollider>().convex = true;
                targetObj.AddComponent<Rigidbody>();
                targetObj.GetComponent<MovedObject>().ChangedState(state);
            }
        }
        // 그랩 대상의 부모가 있다면
        else
        {
            // 고정형 오브젝트 경우
            if (targetObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
            {                
                // 고정형 부모 오브젝트에 클래스를 검사하여 그랩 취소
                if(targetObj.transform.parent.gameObject.GetComponent<CatchObject>() != null)
                {
                    state.onGrab = false;
                    return;
                }

                // 종속해제
                targetObj.transform.parent = null;
                targetObj.GetComponent<MeshCollider>().convex = true;
                targetObj.AddComponent<Rigidbody>();
                targetObj.GetComponent<MovedObject>().ChangedState(state);

            }
            // 상위 오브젝트 경우
            else if (targetObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("GrabedObject"))
            {                
                // 그랩 대상을 상위 오브젝트로 변경
                targetObj = targetObj.transform.parent.gameObject;
                CatchObject controll = targetObj.GetComponent<CatchObject>();
                controll.SetUpMesh();
                targetObj.AddComponent<Rigidbody>();                
                controll.ChangedState(state);
            }
        }
        
        targetRigid = targetObj.GetComponent<Rigidbody>();        
        state.pickupPoint.position = state.hit.point;
        followPos = state.pickupPoint.position - state.hit.transform.position;
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;
        state.isShootingState = true;

        if (state.Ammo > -ammo)
        {
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

        if (!state.CanFire || hit.transform?.gameObject == state.hit.transform?.gameObject || !state.UsedGrabGun())
        {
            return false;        
        }

        return true;
    }

}
