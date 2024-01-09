
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

    private void OnDisable()
    {
        //instance = null;
    }

    public int GrabShot { get { return -ammo; } set { ammo = -value; } }

    GameObject targetObj = null;
    Rigidbody targetRigid = null;           
    Vector3 offset;
    
    float maxSpeed = 3f;
    public override bool ShootGun()
    {
        // 언락전 우클릭 입력시 UI 텍스트 출력
        if(!state.UsedGrabGun())
        {
            if(state.CanFire)
            {
                state.CheckUnlockUi();       
                state.FadeOutCrossHair();
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
        // TODO : 임시야매로 그랩건의 오브젝트 그랩 남발 방지
        else if (state.hit.transform.gameObject.layer != LayerMask.NameToLayer("MovedObject") &&
            state.hit.transform.gameObject.layer != LayerMask.NameToLayer("GrabedObject") ||
            state.hit.transform.parent?.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {            
            return false;
        }

        FollowingObj();
        return true;
    }

    void FixedUpdate()
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

            //Vector3 wantValue = state.pickupPoint.position + (state.cameraController.GetGrabOffset(offset));
            //Vector3 up = Vector3.Cross(state.pickupPoint.right, state.pickupPoint.forward);
            //Vector3 right = Vector3.Cross(state.pickupPoint.up, state.pickupPoint.forward);
            //float distance = Vector3.Distance(target, pickup);

            //Vector3 target = targetRigid.position - targetRigid.position.y * Vector3.up;
            //Vector3 pickup = state.pickupPoint.position - state.pickupPoint.position.y * Vector3.up;

            state.cameraController.RotateSomethingAtCameraCenter(state.grabCorrectPoint);
            Vector3 dir = state.grabCorrectPoint .position -  targetRigid.position;

            state.grabLine.enabled = true;
            state.grabLine.SetPosition(0, state.GunHolderHand.position);
            state.grabLine.SetPosition(1, state.pickupPoint.position);

            targetRigid.velocity = dir * 10;
            //targetRigid.rotation = state.grabCorrectPoint.rotation;
            //targetRigid.velocity += state.pickupPoint.forward * distance;
            //targetRigid.velocity =  up* -dir.y * 3 + right* dir.x * 3 + state.pickupPoint.forward * dir.z * 3;
        }
    }

    public void CancelObj()
    {        
        if(targetRigid != null)
        {            
            targetRigid.constraints = RigidbodyConstraints.None;
            targetRigid.useGravity = true;            
            targetRigid.velocity = Vector3.down * 2f;
            targetRigid = null;            

        }

        if (targetObj?.GetComponent<MeshCollider>() != null)
        {
            targetObj.GetComponent<MeshCollider>().material.dynamicFriction = 1f;

            if (targetObj.GetComponent<MovedObject>() != null)
            {
                targetObj.GetComponent<MovedObject>().CancelGrab();
            }

        }

        if (targetObj?.GetComponent<CatchObject>() != null)
        {
            targetObj.GetComponent<CatchObject>().CancelGrab();
        }


        state.cameraController.ClearGrabObject();
        targetObj = null;
        state.grabLine.enabled = false;
        state.onGrab = false;        
    }

    public void CancleGrab()
    {
        state.cameraController.ClearGrabObject();
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
                targetObj.GetComponent<MovedObject>().ChangedState();
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
                targetObj.GetComponent<MovedObject>().ChangedState();

            }
            // 상위 오브젝트 경우
            else if (targetObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("GrabedObject"))
            {                
                // 그랩 대상을 상위 오브젝트로 변경
                targetObj = targetObj.transform.parent.gameObject;
                CatchObject controll = targetObj.GetComponent<CatchObject>();
                controll.SetUpMesh();
                targetObj.AddComponent<Rigidbody>();                
                controll.ChangedState();
            }

        }
        

       // state.cameraController.SetGrabObject(targetObj.transform);
        
        targetRigid = targetObj.GetComponent<Rigidbody>();
        state.pickupPoint.position = state.hit.point;
        state.cameraController.SetGrabObject(targetObj.transform);
        state.grabCorrectPoint.position = targetRigid.position;
        //targetRigid = targetObj.GetComponent<Rigidbody>();

        // 픽업 포인트는 해당 오브젝트의 중심부
        //state.pickupPoint.position = targetRigid.position;

        // 조합된 오브젝트일경우 픽업포인트 맞추기
        //if (targetObj.GetComponent<CatchObject>() != null)
        {            
        //    state.pickupPoint.position = state.hit.point;
        }

        //followPos = state.pickupPoint.position - state.hit.transform.position;
        
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;      
        state.isShootingState = true;

        if (state.Ammo >= -ammo)
        {
            UsedAmmo(ammo);
        }

    }

    float distance;

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
