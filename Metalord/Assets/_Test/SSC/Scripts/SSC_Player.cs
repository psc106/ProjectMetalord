using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_Player : MonoBehaviour
{ 
    Vector3 dir;
    Transform body;
    Rigidbody myRigid;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        body = transform.GetChild(0).GetComponentInChildren<Transform>();
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);

        myRigid.velocity = move * speed;
        move = transform.TransformDirection(move);

        if(move.x != 0 || move.z != 0)
        {
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.LookRotation(move), 3f * Time.deltaTime);
        }
        
    }

}
