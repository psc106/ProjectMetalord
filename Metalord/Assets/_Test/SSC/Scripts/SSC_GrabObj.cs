using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_GrabObj : MonoBehaviour
{
    [SerializeField] private LayerMask grabObj;
    [SerializeField] private Transform grabPos;
    Camera cam;

    Transform objTrans;
    Rigidbody objRigid;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hitInfo;            

            if(Physics.Raycast(ray, 100f, grabObj) == true)
            {
                Physics.Raycast(ray, out hitInfo);

                grabLine.enabled = true;
                objTrans = hitInfo.transform;
                objRigid = hitInfo.rigidbody;                

            }

        }

        if(objTrans != null)
        {            
            objRigid.useGravity = false;
            DrawLine();
            Vector3 moveVec = Vector3.Lerp(objTrans.position, grabPos.position, Time.deltaTime * 2f);
            objTrans.position = moveVec;

            if(Input.GetMouseButtonUp(1))
            {
                ClearObj(objTrans.position);
            }
        }

    }

    public Transform lineStart;
    public Transform curvePoint;
    public Transform lineEnd;
    public LineRenderer grabLine;
    List<Vector3> curvePos = new List<Vector3>();

    void DrawLine()
    {
        lineEnd = objTrans;
        curvePos.Clear();

        for (float i = 0; i <= 1; i += 0.01f)
        {
            Vector3 tan1 = Vector3.Lerp(lineStart.position, curvePoint.position, i);
            Vector3 tan2 = Vector3.Lerp(curvePoint.position, lineEnd.position, i);

            Vector3 curve = Vector3.Lerp(tan1, tan2, i);

            curvePos.Add(curve);
        }

        grabLine.positionCount = curvePos.Count;
        grabLine.SetPositions(curvePos.ToArray());

    }

    void ClearObj(Vector3 currentObjPos)
    {
        Vector3 objDistance = currentObjPos - grabPos.position;
        Vector3 movePos = grabPos.forward + objDistance;
        Vector3 moveForce = movePos - currentObjPos;

        grabLine.enabled = false;
        objRigid.useGravity = true;
        objRigid.velocity = moveForce * -1f;
        objTrans = null;
        objRigid = null;        
    }
}
