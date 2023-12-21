using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    protected GunStateController state;
    protected Brush brush = null;
    protected Transform AimTarget = null;
    protected int ammo;
    public LayerMask myLayer;

    [HideInInspector] public bool OnGrab = false;

    protected virtual void Awake()
    {        
        brush = new Brush();
        state = FindAnyObjectByType<GunStateController>();

        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;

            brush.splatScale = 10;
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
        PaintTarget.PaintRay(_ray, brush, state.range);

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
}
