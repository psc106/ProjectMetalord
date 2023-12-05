using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class PlayerCameraTest : MonoBehaviour
{
    public Legacy_PlayerValue playerValue;
    public Transform target;

    Vector3 offsetNormalize;
    float offsetMagnitude;

    [SerializeField]
    float followSpeed;
    [SerializeField]
    float rotateSpeedX;
    [SerializeField]
    float rotateSpeedY;

    float rotateX;
    float rotateY;

    private void Start()
    {
       // Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        offsetNormalize = transform.rotation.eulerAngles.normalized;
        offsetMagnitude = (target.position - transform.position).magnitude;

        rotateX = transform.rotation.eulerAngles.x;
        rotateY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        rotateY += Input.GetAxis("Mouse X") * rotateSpeedY * Time.deltaTime;
        rotateX += -Input.GetAxis("Mouse Y") * rotateSpeedX * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, -70, 70);

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed*Time.deltaTime);
        Vector3 forword = Vector3.Scale(transform.forward, new Vector3(1,0,1));
        playerValue.oriented.forward = forword;
    }
}
