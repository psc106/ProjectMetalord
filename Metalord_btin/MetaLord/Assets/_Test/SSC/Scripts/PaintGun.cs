using UnityEngine;
using UnityEngine.Windows;

public class PaintGun : GunBase
{
    float autoTime = 1f;
    float rangeLimit = 4f;
    float timeCheck = 0f;
    float autotimeCheck = 0;    
    int autoShot = -5;
    bool fireStart = false;

    int paintAmmo
    {
        get
        {
            if(fireStart)
            {
                return autoShot;
            }

            return ammo;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        mode = GunMode.Paint;
        brush.splatChannel = 0;
        brush.splatScale = state.ClimbeSize;
        ammo = -50;

        AimTarget = state.AimTarget;
    }


    public override void ShootGun()
    {
        if(CheckCanFire() == false || !state.CanFire)
        {
            fireStart = false;
            autotimeCheck = 0f;
            return;
        }

        // 마우스 클릭에서 손을 떼면 사격 중지.
        if (!state.reader.ShootKey)
        {
            fireStart = false;
            autotimeCheck = 0f;
        }

        // 일정시간동안 사격키 입력상태라면 연사모드
        else if (autotimeCheck > state.AutoInitTime && state.CanFire)
        {
            AutoFire();
        }

        // 사격을 시작 == 마우스버튼 누른시점동안
        else if (fireStart == true)
        {
            autotimeCheck += Time.deltaTime;
        }

        else if (state.reader.ShootKey && state.CanFire)
        {
            NormalFire();
        }
    }


    private void NormalFire()
    {
        if (state.checkSuccessRay)
        {
            if(state.Ammo < -ammo)
            {
                return;
            }

            Ray muzzleRay = new Ray(state.startPoint, state.hit.point - state.startPoint);
/*            if(state.minDistance == true)
            {

                muzzleRay = new Ray(state.startPoint, state.hit.point - state.startPoint);
                UsedAmmo(muzzleRay, paintAmmo);

                fireStart = true;                
                return;
            }*/

            UsedAmmo(muzzleRay, paintAmmo);
           
            fireStart = true;
        }
    }

    private void AutoFire()
    {
        timeCheck += Time.deltaTime;

        if (timeCheck >= state.fireRate)
        {
            if (state.checkSuccessRay)
            {
                Ray muzzleRay = new Ray(state.startPoint, state.hit.point - state.startPoint);

                UsedAmmo(muzzleRay, autoShot);                
                timeCheck = 0f;
            }
        }
    }

    protected override bool CheckCanFire()
    {
        if (!state.CanFire || state.Ammo < -paintAmmo)
        {
            return false;
        }

        return true;
    }

    public override bool CanFireAmmoCount()
    {
        return state.Ammo >= (fireStart && autotimeCheck > state.AutoInitTime ? -autoShot : - ammo);
    }
}
