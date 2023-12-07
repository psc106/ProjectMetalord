using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] InputReader input;
    [SerializeField] Transform targetY;
    [SerializeField] Transform targetX;

    [SerializeField]
    Controller_Physics player;
    [SerializeField]
    Transform crossHair;
    [SerializeField]
    CinemachineVirtualCamera climbCamera;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 3f)] float SpeedMulitiplier = 1f;

    bool isUnLockPressed = false;
    bool cameraMovementLock = false;


    private void OnEnable()
    {
        transform.parent = null;

        input.Look += OnLook;
        input.EnableMouseControlCamera += OnEnableMouseControlCamera;
        input.DisableMouseControlCamera += OnDisableMouseControlCamera;

        isUnLockPressed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        input.Look -= OnLook;
        input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
        input.DisableMouseControlCamera -= OnDisableMouseControlCamera;

    }

    private void LateUpdate()
    {
        climbCamera.gameObject.SetActive(player.OnClimb);
        crossHair.gameObject.SetActive(!player.OnClimb);


        //y축 변경
        if (player.OnClimb)
        {
            Vector3 lookPoint = player.GetClimbNormal();
            lookPoint.y = 0;
            player.transform.LookAt(player.transform.position - lookPoint);
        }
        else
        {
            targetY.Rotate(Vector3.up, newRotationY);
        }
        //x축 변경
        targetX.rotation = Quaternion.Euler(newRotationX, targetX.eulerAngles.y, targetX.eulerAngles.z);
    }
    float newRotationY;
    float newRotationX;

    void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (cameraMovementLock) return;
        if (isDeviceMouse && isUnLockPressed) return;

        float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

        newRotationY = cameraMovement.x * SpeedMulitiplier * deviceMultiplier;

        
        newRotationX = targetX.eulerAngles.x - cameraMovement.y * SpeedMulitiplier * deviceMultiplier;
        newRotationX = Mathf.Clamp(newRotationX > 180 ? newRotationX - 360 : newRotationX, -89, 89);

        //targetY.Rotate(Vector3.up, newRotationY);


    }

    void OnEnableMouseControlCamera()
    {
        isUnLockPressed = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDisableMouseControlCamera()
    {
        isUnLockPressed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(DisableMouseForFrame());
    }

    IEnumerator DisableMouseForFrame()
    {
        cameraMovementLock = true;
        yield return new WaitForEndOfFrame();
        cameraMovementLock = false;
    }


}
