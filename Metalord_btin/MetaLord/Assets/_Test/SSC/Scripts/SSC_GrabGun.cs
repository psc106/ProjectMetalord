using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class SSC_GrabGun : MonoBehaviour
{
    [SerializeField] private LayerMask grabObj;
    [SerializeField] private Transform grabPos;
    [SerializeField] private Transform grabLimit;
    
    GameObject followObj;
    Transform objTrans;
    Rigidbody objRigid;
    SSC_GrabObj targetObj;

    public Transform lineStart;            
    public LineRenderer grabLine;

    bool isGrab = false;   

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(isGrab == false)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, 100f, grabObj) == true)
                {
                    Physics.Raycast(ray, out hitInfo);

                    InitGrabobj(hitInfo);
                }

                return;
            }        

            if(isGrab == true)
            {                
                ClearObj();
                isGrab = false;
                return;
            }
        }

        if (objTrans != null)
        {                        
            DragObj();            
        }

    }

    void InitGrabobj(RaycastHit hitObj)
    {
        followObj = new GameObject();
        followObj.transform.position = hitObj.point;
        hitObj.transform.parent = followObj.transform;

        targetObj = hitObj.transform.GetComponent<SSC_GrabObj>();
        MeshCollider targetColid = hitObj.transform.GetComponent<MeshCollider>();
        targetColid.convex = true;
        hitObj.transform.AddComponent<Rigidbody>();
        grabLine.enabled = true;
        objTrans = hitObj.transform;
        objRigid = hitObj.rigidbody;
        grabPos.position = hitObj.point;
        targetObj.ChangedState(false);        
        
        isGrab = true;
    }

    void DragObj()
    {
        float t = Input.GetAxisRaw("Mouse ScrollWheel");

        if (t < 0f)
        {
            if (Vector3.Distance(grabPos.position, grabLimit.position) <= 10f)
            {
                return;
            }

            Vector3 resultPos = grabLimit.position - grabPos.position;

            grabPos.transform.position -= resultPos.normalized * t * 5f;

        }

        grabLine.SetPosition(0, lineStart.position);
        grabLine.SetPosition(1, followObj.transform.position);

        Vector3 moveVec = Vector3.Lerp(followObj.transform.position, grabPos.position, Time.deltaTime * 2f);
        followObj.transform.position = moveVec;            
    }

    void ClearObj()
    {
        targetObj.transform.parent = null;
        Destroy(followObj);
        objRigid.useGravity = true;
        targetObj = null;
        grabLine.enabled = false;                       
        objTrans = null;
        objRigid = null;
    }
}
