using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SSC_GrabGun : MonoBehaviour
{    
     
    Rigidbody targetRigid;
    [Range(1f, 100f), SerializeField] private float range;
    [SerializeField] private LayerMask MovedObject;
    [SerializeField] private Transform pickupPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform followObj;
    [SerializeField] private LineRenderer grabLine;
    [SerializeField] private SSC_GunState state;

    GameObject targetObj;

    int grabAmmo = -55;
    float rangeLimit = 20f;
    [HideInInspector] public bool OnGrab = false;

    Vector3 followPos;

    private void Start()
    {
        range = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(GetOriginPos(), CheckDir());        

        if (Input.GetMouseButtonDown(0))
        {
            GrabbingObj(ray);
        }

    }

    public void GrabbingObj(Ray ray)
    {
        RaycastHit hitInfo;

        if (targetRigid)
        {
            CancelObj();
            return;
        }

        if (state.Ammo < -grabAmmo || !state.CanFire)
        {
            return;
        }

        if (Physics.Raycast(ray, out hitInfo, range, MovedObject))
        {
            float distanceCheck = Vector3.Distance(startPoint.position, hitInfo.point);
            float distanceCheck2 = Vector3.Distance(startPoint.position, hitInfo.transform.position);

            if (distanceCheck <= rangeLimit ||
                distanceCheck2 <= rangeLimit
               )
            {
                return;
            }

            OnGrab = true;
            FollowingObj(hitInfo);
        }
    }

    private void FixedUpdate()
    {
        if (targetRigid)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");

            if (wheel < 0f)
            {
                PulledObj(wheel);
            }            

            Vector3 dir = (pickupPoint.position - followPos) - targetRigid.position;
            float mag = dir.magnitude;

            grabLine.enabled = true;
            grabLine.SetPosition(0, startPoint.position);
            grabLine.SetPosition(1, followObj.transform.position);

            targetRigid.velocity = dir * mag;

            float distanceCheck = Vector3.Distance(startPoint.position, targetObj.transform.position);            
            float distanceCheck2 = Vector3.Distance(startPoint.position, pickupPoint.position);

            if (distanceCheck <= rangeLimit
                || distanceCheck2 <= rangeLimit)
            {
                CancelObj();
            }
        }
    }

    void FollowingObj(RaycastHit hitInfo)
    {
        targetObj = hitInfo.transform.gameObject;
        followObj.position = hitInfo.point;
        followPos = followObj.position - hitInfo.transform.position;
        hitInfo.transform.GetComponent<MeshCollider>().convex = true;
        hitInfo.transform.AddComponent<Rigidbody>();
        targetRigid = hitInfo.rigidbody;
        hitInfo.transform.GetComponent<SSC_GrabObj>().ChangedState();
        targetRigid.constraints = RigidbodyConstraints.FreezeRotation;
        targetRigid.useGravity = false;

        state.UpdateState(grabAmmo);
    }

    public void CancelObj()
    {
        targetObj = null;
        grabLine.enabled = false;
        targetRigid.constraints = RigidbodyConstraints.None;
        targetRigid.useGravity = true;
        targetRigid.velocity = Vector3.zero;
        targetRigid = null;
        OnGrab = false;
        state.UpdateState(-grabAmmo);
    }

    void PulledObj(float wheelData)
    {      
        Vector3 dir = startPoint.localPosition - pickupPoint.localPosition;

        if(Vector3.Distance(pickupPoint.position, startPoint.position) < 25f)
        {
            return;
        }

        dir = dir.normalized * wheelData;
        pickupPoint.transform.localPosition -= dir;
    }

    Vector3 GetOriginPos()
    {
        Vector3 origin = Vector3.zero;

        origin = Camera.main.transform.position +
            Camera.main.transform.forward *
            Vector3.Distance(Camera.main.transform.position, startPoint.position);

        return origin;
    }

    Vector3 CheckDir()
    {
        Vector3 dir = Vector3.zero;
        dir = Camera.main.transform.forward +
            Camera.main.transform.TransformDirection(dir);

        return dir;
    }
}
