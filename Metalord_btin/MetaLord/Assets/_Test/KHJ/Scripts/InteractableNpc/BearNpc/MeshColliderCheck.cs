using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderCheck : MonoBehaviour
{
    public MeshCollider testMeshCollider;
    // Start is called before the first frame update
   
    void Start()
    {
        Debug.Log(testMeshCollider.name);
        Debug.Log(testMeshCollider.bounds);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) 
        {
            Ray ray = Camera.main .ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                Debug.Log(hit.collider.GetType().ToString());

            }
        }
    }
}
