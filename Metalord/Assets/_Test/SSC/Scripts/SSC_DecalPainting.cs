using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_DecalPainting : MonoBehaviour
{
    [SerializeField] private GameObject Decal;        

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitinfo;

            if(Physics.Raycast(ray, out hitinfo))
            {
                GameObject obj = Instantiate(Decal, hitinfo.point, Quaternion.FromToRotation(Vector3.up, hitinfo.normal));

                obj.transform.parent = hitinfo.transform;
            }
        }
    }
}
