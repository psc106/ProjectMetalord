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
    [SerializeField] CinemachineFreeLook freeLookCam;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 3f)] float SpeedMulitiplier = 1f;

    bool isUnLockPressed = false;
    bool cameraMovementLock = false;


    private void OnEnable()
    {
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

    void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (cameraMovementLock) return;
        if (isDeviceMouse && isUnLockPressed) return;

        float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
        //freeLookCam.m_XAxis.m_InputAxisValue = cameraMovement.x * SpeedMulitiplier * deviceMultiplier;
        //freeLookCam.m_YAxis.m_InputAxisValue = cameraMovement.y * SpeedMulitiplier * deviceMultiplier;

        targetY.Rotate(Vector3.up, cameraMovement.x * SpeedMulitiplier * deviceMultiplier);
        //targetX.Rotate(Vector3.right, cameraMovement.y * SpeedMulitiplier * deviceMultiplier);

        // Adjust the Y axis rotation based on input, and limit it within minYRotation and maxYRotation
        float newRotationX = targetX.eulerAngles.x - cameraMovement.y * SpeedMulitiplier * deviceMultiplier;
        newRotationX = Mathf.Clamp(newRotationX > 180 ? newRotationX - 360 : newRotationX, -89, 89);

        targetX.rotation = Quaternion.Euler(newRotationX, targetX.eulerAngles.y, targetX.eulerAngles.z);

    }

    void OnEnableMouseControlCamera()
    {
        isUnLockPressed = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        freeLookCam.m_XAxis.m_InputAxisValue = 0f;
        freeLookCam.m_YAxis.m_InputAxisValue = 0f;
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
