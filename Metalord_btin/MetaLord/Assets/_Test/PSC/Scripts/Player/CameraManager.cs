using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    CinemachineVirtualCamera groundCamera;
    [SerializeField]
    CinemachineVirtualCamera climbCamera;
    [SerializeField]
    CinemachineVirtualCamera blendCamera;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 20f)] float SpeedMulitiplier = 1f;
    [SerializeField, Range(0, 10)]     float blendCameraDuration = 1f;

    bool isUnLockPressed = false;

    float fixedAngle = -1;
    float time;

    float newRotationY;
    float newRotationX;
    static public bool cameraMovementLock { get; private set; }



    private void OnEnable()
    {
        cameraMovementLock = false;
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

    private void Update()
    {
        crossHair.gameObject.SetActive(!player.OnClimb);
    }

    public void UpdateFixedAngle()
    {
        time = 0;
        Vector3 currAngle = player.GetPreviousClimbNormal();
        currAngle.y = 0;
        currAngle.Normalize();
        Quaternion rotation = Quaternion.LookRotation(currAngle);
        fixedAngle = rotation.eulerAngles.y;
    }


    private void LateUpdate()
    {

        if (cameraMovementLock) return;

        if (fixedAngle != -1 && blendCameraDuration >= time)
        {
            time += Time.deltaTime;
            float currAngle = Mathf.Lerp(targetY.eulerAngles.y, fixedAngle, time / blendCameraDuration);

            targetY.transform.eulerAngles = new Vector3(0, currAngle, 0);
            targetX.rotation = Quaternion.Euler(targetX.eulerAngles.x, currAngle, targetX.eulerAngles.z);
        }

        if (input.mouseMovement.magnitude == 0 && input.Direction.magnitude == 0) 
        {
            newRotationY = targetX.eulerAngles.y;
            newRotationX = targetX.eulerAngles.x;
            return;
        }
        
        if (input.mouseMovement.magnitude != 0)
        {
            fixedAngle = -1;
            time = 0;
        }
        
        //Vector2 cameraMovement = input.mouseMovement;

        if (player.OnClimb)
        {
            Vector3 currAngle = -player.GetClimbNormal();
            currAngle.y = 0;
            currAngle.Normalize();
            Quaternion rotation = Quaternion.LookRotation(currAngle);
            float anchor = rotation.eulerAngles.y;

            if (anchor - 89 < 0)
            {
                if (newRotationY > 180)
                {
                    newRotationY = Mathf.Clamp(newRotationY, 360 + (anchor - 89), newRotationY);
                }
                else
                {
                    newRotationY = Mathf.Clamp(newRotationY, newRotationY, (anchor + 89));
                }
            }
            else if (anchor + 89 > 360)
            {
                if (newRotationY < 180)
                {
                    newRotationY = Mathf.Clamp(newRotationY, newRotationY, anchor + 89 - 360);
                }
                else
                {
                    newRotationY = Mathf.Clamp(newRotationY, (anchor - 89), newRotationY);
                }
            }
            else
            {
                newRotationY = Mathf.Clamp(newRotationY, (anchor - 89), (anchor + 89));
            }
        }
        targetY.transform.eulerAngles = new Vector3(0, newRotationY, 0);
        //targetY.transform.eulerAngles = new Vector3(0, Mathf.Lerp(targetY.transform.eulerAngles.y, newRotationY, 15 * Time.deltaTime), 0);

        //y축 변경
        //x축 변경
        //targetX.rotation = Quaternion.Euler(newRotationX, Mathf.Lerp(targetX.eulerAngles.y, newRotationY, 15 * Time.deltaTime), targetX.eulerAngles.z);
        targetX.rotation = Quaternion.Euler(newRotationX, newRotationY, targetX.eulerAngles.z);
    }
    void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (cameraMovementLock) return;
        if (isUnLockPressed) return;
        newRotationY = targetX.eulerAngles.y + cameraMovement.x * SpeedMulitiplier * Time.deltaTime;
        newRotationX = targetX.eulerAngles.x - cameraMovement.y * SpeedMulitiplier * Time.deltaTime;
        newRotationX = Mathf.Clamp(newRotationX > 180 ? newRotationX - 360 : newRotationX, -89, 89);
    }


    void OnEnableMouseControlCamera()
    {
        if (cameraMovementLock) return;
        isUnLockPressed = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDisableMouseControlCamera()
    {
        if (cameraMovementLock) return;
        isUnLockPressed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    public void ChangePriorityCamera(CameraType type ,int priority)
    {
        switch (type)
        {
            case CameraType.Ground:
                groundCamera.Priority = priority;
                break;
            case CameraType.Blend:
                blendCamera.Priority = priority;
                break;
            case CameraType.Climb:
                climbCamera.Priority = priority;
                break;
        }
    }

    public void PlayBlendCameraRoutine()
    {
        StartCoroutine(BlendCameraRoutine(blendCameraDuration));
    }


    IEnumerator BlendCameraRoutine(float time)
    {
        blendCamera.Priority = 100;
        yield return new WaitForSeconds(time);
        blendCamera.Priority = 1;

    }




    public static void SwitchCameraLock(bool check)
    {
        cameraMovementLock = check;
        if(check) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }


}

public enum CameraType
{
    Ground, Climb, Blend
}
