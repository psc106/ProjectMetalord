using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SSC_GrabGun : MonoBehaviour
{
    [SerializeField] private LayerMask grabObj;
    [SerializeField] private Transform grabPos;
    [SerializeField] private Transform grabLimit;

    MeshCollider childColid;

    Transform objTrans;
    Rigidbody objRigid;
    SSC_GarbObj targetObj;

    public Transform lineStart;            
    public LineRenderer grabLine;

    bool isGrab = false;

    Vector3 hitPos;

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
                    Debug.Log(hitInfo.transform.name);
                    targetObj = hitInfo.transform.GetComponent<SSC_GarbObj>();

                    //childColid = hitInfo.transform.GetChild(0).transform.GetComponent<MeshCollider>();
                    //childColid.enabled = false;
                    grabLine.enabled = true;                
                    objTrans = hitInfo.transform;
                    objRigid = hitInfo.rigidbody;                                        
                    objRigid.useGravity = false;
                    //objRigid.isKinematic = false;
                    targetObj.ChangedState(false);
                    objRigid.constraints = RigidbodyConstraints.FreezeRotation;

                    grabPos.position = hitInfo.point;

                    hitPos = hitInfo.point - objTrans.position;
                    isGrab = true;
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
            float t = Input.GetAxisRaw("Mouse ScrollWheel");   
            
            if(t < 0f)
            {                
                if(Vector3.Distance(grabPos.position, grabLimit.position) <= 10f)
                {
                    return;
                }

                Vector3 resultPos = grabLimit.position - grabPos.position;

                grabPos.transform.position -= resultPos.normalized * t * 5f;                
                
            }
            
            DrawLine();
            
            Vector3 moveVec = Vector3.Lerp(objTrans.position, grabPos.position, Time.deltaTime * 2f);
            objTrans.position = moveVec;
        }

    }

    void DrawLine()
    {
        Vector3 resultPos = objTrans.position + hitPos;             
        grabLine.SetPosition(0, lineStart.position);
        grabLine.SetPosition(1, objTrans.position);
    }

    void ClearObj()
    {
        //targetObj.ChangedState(false);
        //childColid.enabled = true;
        targetObj = null;
        grabLine.enabled = false;        
        
        objRigid.useGravity = true;
        objTrans = null;
        //objRigid = null;
    }
}
