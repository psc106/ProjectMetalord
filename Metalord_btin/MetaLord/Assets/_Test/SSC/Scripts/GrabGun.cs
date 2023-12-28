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

            if(targetObj.transform.childCount != 0)
            {
                for(int i = 0; i < targetObj.transform.childCount; i++)
                {
                    targetObj.transform.GetChild(i).GetComponent<MeshCollider>().material.dynamicFriction = 1f;
                }

            }
            else
            {
                targetObj.GetComponent<Collider>().material.dynamicFriction = 1f;
            }
        }

        targetObj = null;
        state.grabLine.enabled = false;
        state.onGrab = false;        
    }

    void FollowingObj( )
    {
        state.onGrab = true;
        targetObj = state.hit.transform.gameObject;
        Debug.Log("가장 처음 : " + targetObj);

        // 합쳐진 오브젝트가 그랩 되었을 때
        if(targetObj.transform.parent != null)
        {
            Debug.Log("1 " + targetObj);
            switch (LayerMask.LayerToName(targetObj.transform.parent.gameObject.layer))
            {
                case "CatchObject":
                    targetObj = targetObj.transform.parent.gameObject;
                    int count = targetObj.transform.childCount;

                    for (int i = 0; i < count; i++)
                    {
                        Destroy(targetObj.transform.GetChild(i).GetComponent<Rigidbody>());
                        targetObj.transform.GetChild(i).GetComponent<MeshCollider>().convex = true;
                    }

                    targetObj.AddComponent<Rigidbody>();
                    if (targetObj.GetComponent<MovedObject>() == null)
                    {
                        targetObj.AddComponent<MovedObject>().InitParentMovedObject();
                    }
                    break;
                default:
                    targetObj.transform.parent = null;
                    targetObj.GetComponent<MovedObject>().Invoke("StateChagned", 2f);
                    state.hit.transform.GetComponent<MeshCollider>().convex = true;
                    state.hit.transform.AddComponent<Rigidbody>();                    
                    state.hit.transform.GetComponent<MovedObject>().ChangedState();
                    break;
            }
        }
        else
        {
            // 합쳐진 오브젝트들을 부모 오브젝트로만 인식하게 될 때
            if (targetObj.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
            {
                Debug.Log("2 " + targetObj);
                int count = targetObj.transform.childCount;

                for (int i = 0; i < count; i++)
                {
                    if (targetObj.transform.GetChild(i).GetComponent<Rigidbody>() != null)
                    {
                        Destroy(targetObj.transform.GetChild(i).GetComponent<Rigidbody>());
                    }

                    targetObj.transform.GetChild(i).GetComponent<MeshCollider>().convex = true;
                }

                targetObj.AddComponent<Rigidbody>();
                  
                if (targetObj.GetComponent<MovedObject>() == null)
                {                    
                    targetObj.AddComponent<MovedObject>().InitParentMovedObject();
                }
            }
            else
            {
                Debug.Log("3 " + targetObj);
                targetObj.GetComponent<MovedObject>().Invoke("StateChagned", 2f);
                state.hit.transform.GetComponent<MeshCollider>().convex = true;
                state.hit.transform.AddComponent<Rigidbody>();
                state.hit.transform.GetComponent<MovedObject>().ChangedState();
            }
        }

        targetRigid = targetObj.GetComponent<Rigidbody>();
        state.pickupPoint.position = state.hit.point;
        followPos = state.pickupPoint.position - state.hit.transform.position;
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;

        if(state.Ammo > -ammo)
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

        // 플레이어 밑에 있는 오브젝트가 내 조준점에 담긴 오브젝트라면 그랩시도 X
        if (!state.CanFire || hit.transform?.gameObject == state.hit.transform?.gameObject)
        {            
            return false;        
        }

        return true;
    }

}
