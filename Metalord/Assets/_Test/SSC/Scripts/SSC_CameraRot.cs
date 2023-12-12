using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SSC_CameraRot : MonoBehaviour
{
    private void Update()
    {        
        Vector2 mouseMove = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * 500f * Time.deltaTime;
        Vector3 camera = transform.rotation.eulerAngles;

        mouseMove.x = Mathf.Clamp(mouseMove.x, -90f, 90f);
        transform.rotation = Quaternion.Euler(camera.x + mouseMove.x, camera.y + mouseMove.y, camera.z);
        
    }
}
