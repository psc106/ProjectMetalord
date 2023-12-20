using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    public GunStateController state;
    protected Brush brush = null;
    protected Transform AimTarget = null;
    protected int ammo;

    public LayerMask myLayer;
    public bool OnGrab = false;

    protected virtual void Awake()
    {
        //state = FindAnyObjectByType<GunStateController>();
        brush = new Brush();

        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;

            brush.splatScale = 10;
        }
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
