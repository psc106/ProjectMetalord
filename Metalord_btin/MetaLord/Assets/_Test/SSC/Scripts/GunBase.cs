using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    protected GunStateController state;
    protected Brush brush = null;
    protected Transform AimTarget = null;
    protected int ammo;
    public LayerMask myLayer;
    
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

        PaintTarget.PaintRay(_ray, brush, myLayer, state.range);

        state.UpdateState(_ammo);

        if (state.Ammo <= 0)
        {
            state.UpdateState(0, GunState.EMPTY);
        }
    }

    protected virtual bool CheckCanFire()
    {
        if(!state.CanFire || state.Ammo < -ammo)
        {
            return false;
        }

        return true;
    }

    abstract public void ShootGun();

    public virtual bool CanFireAmmoCount()
    {
        return state.Ammo >= -ammo;
    }
}
