using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    protected Rigidbody conectedBody;
    protected GunStateController state;
    protected Brush brush = null;
    protected Transform AimTarget = null;
    protected int ammo;
    public bool fireStart = false;
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
        }
    }
    abstract public void ShootGun();
    
    protected void UsedAmmo(Ray _ray, int _ammo)
    {
        int id = (int)GunSoundList.FireSound;
        SoundManager.instance.PlaySound(GroupList.Gun, id);

        // 여기 NPC 체크하는 구간 아예 필요 없을듯?
        // PaintTarget에서 SpherCastAll로 NPC 체크중
        // LEGACY

        //Collider[] npcCheck = Physics.OverlapSphere(state.hit.point, 20f, 1 << LayerMask.NameToLayer("NPC"));
        //foreach (Collider c in npcCheck)
        //{
        //    if (c != null)
        //    {
        //        Debug.Log("NPC 체크?");
        //        BoxCollider interactZone = c.GetComponent<BoxCollider>();
        //        interactZone.enabled = false;
        //        StopLerpGaguge();
        //        shootCoroutine = StartCoroutine(LerpGauge(_ammo));
        //        PaintTarget.PaintRay(_ray, brush, myLayer, state.range);
        //        interactZone.enabled = true;
        //        return;
        //    }
        //}

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

        if (shootCoroutine != null) { StopCoroutine(shootCoroutine); }
        shootCoroutine = StartCoroutine(LerpGauge(_ammo));
    }

    protected virtual bool CheckCanFire()
    {
        if(!state.CanFire || !CanFireAmmoCount())
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
                yield return Time.deltaTime;
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
                yield return Time.deltaTime;
            }
        }
    }
}
