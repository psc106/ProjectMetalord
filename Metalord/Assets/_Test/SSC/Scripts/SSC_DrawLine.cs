using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_DrawLine : MonoBehaviour
{
    public Transform lineStart;
    public Transform curvePoint;
    public Transform lineEnd;
    public LineRenderer grabLine;
    List<Vector3> curvePos = new List<Vector3>();

    void DrawLine()
    {
        //lineEnd = objPos;
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
    void Update()
    {

        DrawLine();
        
    }
}
