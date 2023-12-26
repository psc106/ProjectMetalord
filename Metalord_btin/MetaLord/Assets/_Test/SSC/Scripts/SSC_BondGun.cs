using Unity.VisualScripting;
using UnityEngine;

public class SSC_BondGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;
    [SerializeField] private Transform checkPos;
    [SerializeField] private Transform startPoint;
    [SerializeField] private LayerMask gunLayer;

    float range = 50f;
    int bondAmmo = -60;

    private void Awake()
    {
        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;
        }
    }

    private void Update()
    {
        Ray normalRay = new Ray(GetOriginPos(), CheckDir());
        Ray checkRay = new Ray(checkPos.position, CheckDir());

        if(Input.GetMouseButtonDown(0))
        {
            ShootBond(normalRay, checkRay);
        }
    }

    public void ShootBond(Ray normalRay, Ray CheckRay)
    {        
        if(!gun.CanFire || gun.Ammo < -bondAmmo)
        {
            return;
        }

        if (gun.checkSuccessRay)
        {
            if (gun.hit.transform.GetComponent<NpcBase>() != null)
            {
                gun.hit.transform.GetComponent<BoxCollider>().enabled = false;                
            }

            if (gun.hit.transform.GetComponent<SSC_BondObj>() != null)
            {
                //hit.transform.GetComponent<SSC_BondObj>().myRigid.AddComponent<Rigidbody>();
                SSC_GunState.AddBondList(gun.hit.transform.GetComponent<SSC_BondObj>());
            }

            Ray muzzleRay = new Ray(gun.startPoint, gun.hit.point - gun.startPoint);
            UsedAmmo(muzzleRay, bondAmmo);

            if (gun.hit.transform.GetComponent<NpcBase>() != null)
            {
                gun.hit.transform.GetComponent<BoxCollider>().enabled = true;
            }

        }

        // hit;

        //if (Physics.Raycast(normalRay, out hit, range, gunLayer))
        //{
        //    Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

        //    if (hit.transform.GetComponent<SSC_BondObj>() != null)
        //    {
        //        //hit.transform.GetComponent<SSC_BondObj>().myRigid.AddComponent<Rigidbody>();
        //        SSC_GunState.AddBondList(hit.transform.GetComponent<SSC_BondObj>());                
        //    }

        //    if (gun.hit.transform.GetComponent<NpcBase>() != null)
        //    {
        //        gun.hit.collider.enabled = false;                
        //    }

        //    UsedAmmo(muzzleRay, bondAmmo);

        //    gun.hit.collider.enabled = true;
        //}     
    }

    void UsedAmmo(Ray _ray, int _ammo)
    {
        PaintTarget.PaintRay(_ray, brush, range);
        gun.UpdateState(_ammo);

        if (gun.Ammo <= 0)
        {
            gun.UpdateState(0, GunState.EMPTY);
        }
    }

    Vector3 GetOriginPos()
    {
        Vector3 origin = Vector3.zero;

        origin = Camera.main.transform.position +
            Camera.main.transform.forward *
            Vector3.Distance(Camera.main.transform.position, startPoint.position);

        return origin;
    }

    Vector3 CheckDir()
    {
        Vector3 dir = Vector3.zero;
        dir = Camera.main.transform.forward +
            Camera.main.transform.TransformDirection(dir);

        return dir;
    }

}
