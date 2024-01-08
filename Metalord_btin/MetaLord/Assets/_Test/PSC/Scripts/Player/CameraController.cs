using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class CameraController : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] InputReader input;
    [SerializeField] Transform playerRender;
    [SerializeField] Transform cameraTarget;

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
    [SerializeField]
    Transform mainCameraTransform;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 20f)] float SpeedMulitiplier = 1f;
    [SerializeField, Range(0, 10)]     float blendCameraDuration = 1f;
    [SerializeField, Range(1, 180)]     float smoothRotationSpeed = 5;

    [SerializeField, Range(0, 360)]
    float rotateAngle;
    [SerializeField, Range(0, 10)]
    float rotateTime;

    bool isUnLockPressed = false;

    float fixedAngle = -1;
    float time;
    float currRotateTime = 0;

    float newRotationY;
    float newRotationX;

    Transform grabObject;
    Vector3 grabDiffEuler;
    Vector3 grabOrigin;



    Coroutine rotateCoroutine;
    Quaternion targetRotation;


    private void OnEnable()
    {
        transform.parent = null;

        input.Look += OnLook;
        input.EnableMouseControlCamera += OnEnableMouseControlCamera;
        input.DisableMouseControlCamera += OnDisableMouseControlCamera;

        isUnLockPressed = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(!mainCameraTransform)
            mainCameraTransform = Camera.main.transform;
    }
    private void OnDisable()
    {
        input.Look -= OnLook;
        input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
        input.DisableMouseControlCamera -= OnDisableMouseControlCamera;

    }

    private void Update()
    {
        crossHair.gameObject.SetActive(!player.OnClimbAnimation);
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

        if (Controller_Physics.stopState) return;

        if (fixedAngle != -1 && blendCameraDuration >= time)
        {
            time += Time.deltaTime;
            float currAngle = Mathf.Lerp(playerRender.eulerAngles.y, fixedAngle, time / blendCameraDuration);

            newRotationY = currAngle;
            newRotationX = cameraTarget.eulerAngles.x;
            //cameraTarget.rotation = Quaternion.Euler(newRotationX, newRotationY, cameraTarget.eulerAngles.z);
            return;
        }

        if (input.mouseMovement.magnitude == 0 && input.Direction.magnitude == 0) 
        {
            newRotationY = cameraTarget.eulerAngles.y;
            newRotationX = cameraTarget.eulerAngles.x;
            //cameraTarget.rotation = Quaternion.Euler(newRotationX, newRotationY, cameraTarget.eulerAngles.z);
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

    }
    private void FixedUpdate()
    {
        playerRender.rotation = Quaternion.Euler(0, newRotationY, 0);
        cameraTarget.rotation = Quaternion.Euler(newRotationX, newRotationY, cameraTarget.eulerAngles.z);

        if (grabObject)
        {
            Quaternion grabRotation = Quaternion.Euler(grabOrigin.x, grabDiffEuler.y + newRotationY, grabOrigin.z);
            grabObject.rotation = grabRotation;
        }
    }

    public void SetGrabObject(Transform obj)
    {
        grabObject = obj;
        grabOrigin = grabObject.eulerAngles;
        grabDiffEuler = grabObject.eulerAngles - cameraTarget.eulerAngles;

        //diff = beforeA-beforeB
        //diff = newA-newB
        //diff+beforeB = beforeA
        //diff+newB = newA

    }
    public void ClearGrabObject()
    {
        grabObject = null;
        grabDiffEuler = Vector3.zero;
    }


    void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (Controller_Physics.stopState) return;
        if (isUnLockPressed) return;
        newRotationY = cameraTarget.eulerAngles.y + cameraMovement.x * SpeedMulitiplier * Time.deltaTime;
        newRotationX = cameraTarget.eulerAngles.x - cameraMovement.y * SpeedMulitiplier * Time.deltaTime;
        newRotationX = Mathf.Clamp(newRotationX > 180 ? newRotationX - 360 : newRotationX, -89, 89);
    }


    void OnEnableMouseControlCamera()
    {
        if (Controller_Physics.stopState) return;
        isUnLockPressed = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDisableMouseControlCamera()
    {
        if (Controller_Physics.stopState) return;
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


    IEnumerator RotateBodyRoutine(float newY, float time)
    {
        targetRotation = Quaternion.Euler(0, newY, 0);

        currRotateTime = 0;
        while(currRotateTime <= time)
        {

            playerRender.transform.rotation = Quaternion.Lerp(playerRender.transform.rotation, targetRotation, Time.deltaTime * smoothRotationSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
            currRotateTime += Time.deltaTime;
        }
        rotateCoroutine = null;
        currRotateTime = 0;
    }

    IEnumerator BlendCameraRoutine(float time)
    {
        blendCamera.Priority = 100;
        yield return new WaitForSeconds(time);
        blendCamera.Priority = 1;

    }


}

public enum CameraType
{
    Ground, Climb, Blend
}
