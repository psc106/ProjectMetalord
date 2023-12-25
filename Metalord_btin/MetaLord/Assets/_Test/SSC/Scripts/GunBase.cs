using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    protected GunStateController state;
    protected Brush brush = null;
    protected Transform AimTarget = null;
    protected int ammo;
    public LayerMask myLayer;
    Coroutine shootCoroutine;
    
    public GunMode mode { get; protected set; }

    protected virtual void Awake()
    {        
        brush = new Brush();
        state = FindAnyObjectByType<GunStateController>();

        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;

            brush.splatScale = state.paintingSize;
        }
    }
    abstract public void ShootGun();

    protected void PaintingNpc(Ray muzzleRay, int paintAmmo)
    {
        state.targetNpc = state.hit.transform.GetComponent<NpcBase>();
        BoxCollider interactZone = state.targetNpc.GetComponent<BoxCollider>();
        interactZone.enabled = false;
        UsedAmmo(muzzleRay, paintAmmo);
        state.targetNpc.ChangedState(npcState.glued);
        GunStateController.AddList(state.targetNpc);
        interactZone.enabled = true;
    }
    
    protected void UsedAmmo(Ray _ray, int _ammo)
    {
        int id = (int)GunSoundList.FireSound;
        SoundManager.instance.PlaySound(GroupList.Gun, id);

        // 인스펙터창에서 값 변동 즉시 적용 사항
        brush.splatScale = state.paintingSize;

        // 코루틴 돌고있는지 체크
        StopLerpGaguge();
        shootCoroutine = StartCoroutine(LerpGauge(_ammo));        
        PaintTarget.PaintRay(_ray, brush, myLayer, state.range);        
    }

    // 그랩건일때 레이지점 페인팅 없이 소모값만 적용하고 싶을 때
    protected void UsedAmmo(int _ammo)
    {
        int id = (int)GunSoundList.FireSound;
        SoundManager.instance.PlaySound(GroupList.Gun, id);

        // 인스펙터창에서 값 변동 즉시 적용 사항
        brush.splatScale = state.paintingSize;

        if (shootCoroutine != null) { StopCoroutine(shootCoroutine); }
        shootCoroutine = StartCoroutine(LerpGauge(_ammo));
    }

    protected virtual bool CheckCanFire()
    {
        if(!state.CanFire || state.Ammo < -ammo)
        {
            return false;
        }


        return true;
    }


    public virtual bool CanFireAmmoCount()
    {
        return state.Ammo >= -ammo;
    }

    public void StopLerpGaguge()
    {
        if (shootCoroutine != null) { StopCoroutine(shootCoroutine); }
    }

    IEnumerator LerpGauge(int usingAmmo)
    {
        int currentAmmo = state.Ammo;
        state.checkAmmo += usingAmmo;
        float timeCheck = 0;

        if(state.checkAmmo <= 0)
        {
            state.checkAmmo = 0;
            state.UpdateState(state.checkAmmo, GunState.EMPTY);

            while (timeCheck < state.lerpTime)
            {
                timeCheck += Time.deltaTime;
                float t = timeCheck / state.lerpTime;

                int value = (int)Mathf.Lerp(currentAmmo, state.checkAmmo, t);
                state.UpdateState(value);        
                yield return null;
            }

            yield break;
        }
        else
        {
            while (timeCheck < state.lerpTime)
            {
                timeCheck += Time.deltaTime;
                float t = timeCheck / state.lerpTime;

                int value = (int)Mathf.Lerp(currentAmmo, state.checkAmmo, t);
                state.UpdateState(value);     
                yield return null;
            }
        }


    }
}
