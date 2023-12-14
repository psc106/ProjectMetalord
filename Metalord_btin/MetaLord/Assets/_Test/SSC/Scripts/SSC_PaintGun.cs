using System;
using UnityEngine;

public class SSC_PaintGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;
    [SerializeField] private InputReader input;

    [SerializeField] private LayerMask gunLayer = -1;
    [Range(0.1f, 1f)] public float attackSpeed;

    [SerializeField, Range(0, 1)]
    float autoTime = 1f;
    [SerializeField, Range(1, 100)]
    float range = 50f;

    float timeCheck = 0f;
    float autotimeCheck = 0f;

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
        if (!gun.CanFire)
        {
            fireStart = false;
            autotimeCheck = 0f;
            return;
        }

        // 마우스 클릭에서 손을 떼면 사격 중지.
        if(!input.ShootKey)
        {
            fireStart = false;
            autotimeCheck = 0f;
        }

        // 일정시간동안 사격키 입력상태라면 연사모드
        else if(autotimeCheck > autoTime && gun.state == GunState.READY)
        {
            AutoFire();
            return;
        }

        // 사격을 시작 == 마우스버튼 누른시점동안
        else if(fireStart == true)
        {
            autotimeCheck += Time.deltaTime;
            return;
        }

        else if(input.ShootKey && gun.state == GunState.READY)
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

        if (Physics.Raycast(ray, out hit, range, gunLayer))
        {
            PaintTarget.PaintRay(ray, brush, range);

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

            if (Physics.Raycast(ray, out hit, range, gunLayer))
            {
                PaintTarget.PaintRay(ray, brush, range);

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
