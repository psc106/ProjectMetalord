using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_GunRot : MonoBehaviour
{
    private float cameraSensitivity = 360;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rotationX = transform.eulerAngles.y;
        rotationY = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }
}
