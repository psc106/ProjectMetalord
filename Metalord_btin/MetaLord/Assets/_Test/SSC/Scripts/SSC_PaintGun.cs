using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SSC_PaintGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;

    [Range(0.1f, 1f)] public float attackSpeed;

    float timeCheck = 0f;
    float autotimeCheck = 0f;
    float autoTime = 1f;

    int normalShot = -10;
    int autoShot = -5;
    
    bool fireStart = false;

    private void Awake()
    {
        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭에서 손을 떼면 사격 중지.
        if(Input.GetMouseButtonUp(0))
        {
            fireStart = false;
            autotimeCheck = 0f;
        }

        // 일정시간동안 사격키 입력상태라면 연사모드
        if(autotimeCheck > autoTime && gun.state == GunState.READY)
        {
            AutoFire();
            return;
        }

        // 사격을 시작 == 마우스버튼 누른시점동안
        if(fireStart == true)
        {
            autotimeCheck += Time.deltaTime;            
            return;
        }

        if(Input.GetMouseButtonDown(0) && gun.state == GunState.READY)
        {            
            NormalFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PaintTarget.ClearAllPaint();
            gun.UpdateState(gun.MaxAmmo, GunState.READY);
        }

    }

    /// <summary>
    /// 단발 메소드
    /// </summary>
    private void NormalFire()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            PaintTarget.PaintRay(ray, brush);

            gun.UpdateAmmo(normalShot);

            if (gun.Ammo <= 0)
            {
                gun.UpdateState(0, GunState.EMPTY);
            }

            fireStart = true;
        }
    }

    /// <summary>
    /// 연사 메소드
    /// </summary>
    private void AutoFire()
    {
        timeCheck += Time.deltaTime;

        if(timeCheck >= attackSpeed)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                PaintTarget.PaintRay(ray, brush);

                gun.UpdateAmmo(autoShot);

                if (gun.Ammo <= 0)
                {                    
                    gun.UpdateState(0, GunState.EMPTY);                    
                }
                
                timeCheck = 0f;        
            }
        }
    }
}
