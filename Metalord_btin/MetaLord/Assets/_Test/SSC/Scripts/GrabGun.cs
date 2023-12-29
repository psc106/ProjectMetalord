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
            // onGrab 상태에서도 들고있는 물건 그랩 해제를 위해
            if (targetRigid || state.Ammo < -ammo)
            {
                CancelObj();
                return;
            }

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
        if(targetRigid != null)
        {            
            targetRigid.constraints = RigidbodyConstraints.None;
            targetRigid.useGravity = true;
            targetRigid.velocity = Vector3.zero;
            targetRigid = null;            
        }

        targetObj = null;
        state.grabLine.enabled = false;
        state.onGrab = false;        
    }

    void FollowingObj( )
    {
        state.onGrab = true;
        targetObj = state.hit.transform.gameObject;

        // 그랩 대상의 부모가 없다면
        if (targetObj.transform.parent == null)
        {
            targetObj.GetComponent<MeshCollider>().convex = true;
            targetObj.AddComponent<Rigidbody>();
            targetObj.GetComponent<MovedObject>().ChangedState();
        }
        // 그랩 대상의 부모가 있다면
        else
        {
            // 고정형 오브젝트 경우
            if (targetObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
            {                
                // 종속해제
                targetObj.transform.parent = null;
                targetObj.GetComponent<MeshCollider>().convex = true;
                targetObj.AddComponent<Rigidbody>();
                targetObj.GetComponent<MovedObject>().ChangedState();

            }
            // 상위 오브젝트 경우
            else if (targetObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
            {                
                // 그랩 대상을 상위 오브젝트로 변경
                targetObj = targetObj.transform.parent.gameObject;
                CatchObject controll = targetObj.GetComponent<CatchObject>();
                controll.SetUpMesh();
                targetObj.AddComponent<Rigidbody>();                
                controll.ChangedState();
            }
        }

        targetRigid = targetObj.GetComponent<Rigidbody>();        
        state.pickupPoint.position = state.hit.point;
        followPos = state.pickupPoint.position - state.hit.transform.position;
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;

        if (state.Ammo > -ammo)
        {
            UsedAmmo(ammo);
        }


        //// 가장 처음 탐색하게 될 때 (오브젝트가 기존 레벨에 속해있는지 검출)
        //if(targetObj.transform.parent != null)
        //{            
        //    switch (LayerMask.LayerToName(targetObj.transform.parent.gameObject.layer))
        //    {
        //        // 부모 오브젝트가 합치기 위해 만들어진 오브젝트라면
        //        case "CatchObject":
        //            // 그랩 오브젝트는 부모 오브젝트
        //            targetObj = targetObj.transform.parent.gameObject;
        //            int count = targetObj.transform.childCount;

        //            // 그랩 오브젝트의 자식크기만큼 리지드바디 해제, convex 활성화
        //            // 자신의 리지드바디 활성화
        //            for (int i = 0; i < count; i++)
        //            {
        //                Destroy(targetObj.transform.GetChild(i).GetComponent<Rigidbody>());
        //                targetObj.transform.GetChild(i).GetComponent<MeshCollider>().convex = true;
        //            }
        //            targetObj.AddComponent<Rigidbody>();

        //            if (targetObj.GetComponent<MovedObject>() == null)
        //            {
        //                targetObj.AddComponent<MovedObject>().InitParentMovedObject();
        //            }
        //            break;

        //            // 그외 오브젝트에 속한것이라면 (기존 레벨에 속해있다면)
        //        default:
        //            // 종속 해제, 일정시간 이후부터 충돌 검출
        //            targetObj.transform.parent = null;
        //            targetObj.GetComponent<MovedObject>().Invoke("StateChagned", 2f);
        //            state.hit.transform.GetComponent<MeshCollider>().convex = true;
        //            state.hit.transform.AddComponent<Rigidbody>();                    
        //            state.hit.transform.GetComponent<MovedObject>().ChangedState();
        //            break;
        //    }
        //}
        //// 부모 오브젝트가 없을 때
        //else
        //{
        //    // 합쳐진 오브젝트라면
        //    if (targetObj.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
        //    {
        //        Debug.Log("2 " + targetObj);
        //        int count = targetObj.transform.childCount;

        //        // 자식 수만큼 리지드바디 해제, convex 활성화
        //        // 자신의 리지드바디 활성화
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (targetObj.transform.GetChild(i).GetComponent<Rigidbody>() != null)
        //            {
        //                Destroy(targetObj.transform.GetChild(i).GetComponent<Rigidbody>());
        //            }

        //            targetObj.transform.GetChild(i).GetComponent<MeshCollider>().convex = true;
        //        }

        //        targetObj.AddComponent<Rigidbody>();

        //        if (targetObj.GetComponent<MovedObject>() == null)
        //        {
        //            targetObj.AddComponent<MovedObject>().InitParentMovedObject();
        //        }
        //    }

        //    // 혼자인 오브젝트라면
        //    else
        //    {
        //        Debug.Log("3 " + targetObj);
        //        targetObj.GetComponent<MovedObject>().Invoke("StateChagned", 2f);
        //        state.hit.transform.GetComponent<MeshCollider>().convex = true;
        //        state.hit.transform.AddComponent<Rigidbody>();
        //        state.hit.transform.GetComponent<MovedObject>().ChangedState();
        //    }
        //}
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
