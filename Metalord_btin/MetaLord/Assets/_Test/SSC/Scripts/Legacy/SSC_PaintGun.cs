using System;
using UnityEngine;


public class SSC_PaintGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform aimTarget;

    [SerializeField] private Transform checkPos;
    [SerializeField] private Transform startPoint;

    [SerializeField] private LayerMask gunLayer = -1;
    [Range(0.1f, 1f)] public float attackSpeed;

    [SerializeField, Range(0, 1)]
    float autoTime = 1f;
    [SerializeField, Range(1, 100)]
    float range = 50f;

    float rangeLimit = 4f;

    float timeCheck = 0f;
    float autotimeCheck = 0f;

    int normalShot = -50;
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

        Shoot();

    }

    public void Shoot()
    {
        if (!gun.CanFire)
        {
            fireStart = false;
            autotimeCheck = 0f;
            return;
        }

        // 마우스 클릭에서 손을 떼면 사격 중지.
        if (!input.ShootKey)
        {
            fireStart = false;
            autotimeCheck = 0f;
        }

        // 일정시간동안 사격키 입력상태라면 연사모드
        else if (autotimeCheck > autoTime && gun.CanFire)
        {
            AutoFire();
            return;
        }

        // 사격을 시작 == 마우스버튼 누른시점동안
        else if (fireStart == true)
        {
            autotimeCheck += Time.deltaTime;
            return;
        }

        else if (input.ShootKey && gun.CanFire)
        {
            NormalFire();
        }
    }

    /// <summary>
    /// 단발 메소드
    /// </summary>
    private void NormalFire()
    {
        if (gun.checkSuccessRay)
        {
            if(gun.hit.transform.GetComponent<NpcBase>() != null)
            {
                gun.hit.transform.GetComponent<BoxCollider>().enabled = false;
                gun.hit.transform.GetComponent<NpcBase>().ChangedState(npcState.glued);
                SSC_GunState.AddBondList(gun.hit.transform.GetComponent<NpcBase>());
            }

            Ray muzzleRay = new Ray(gun.startPoint, gun.hit.point - gun.startPoint);
            UsedAmmo(muzzleRay, normalShot);
            Debug.Log(gun.hit.transform.name);

            if (gun.hit.transform.GetComponent<NpcBase>() != null)
            {
                gun.hit.transform.GetComponent<BoxCollider>().enabled = true;
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

        if (timeCheck >= attackSpeed)
        {
            if (gun.checkSuccessRay)
            {
                Ray muzzleRay = new Ray(gun.startPoint, gun.hit.point - gun.startPoint);
                UsedAmmo(muzzleRay, autoShot);

                timeCheck = 0f;
            }
        }
    }

   /* /// <summary>
    /// 카메라와 플레이어의 축을 동일선상에 놓아주는 메소드
    /// </summary>
    /// <returns>카메라의 정면 방향 + 플레이어의 수직선상 </returns>
    Vector3 GetOriginPos()
    {
        Vector3 origin = Vector3.zero;

        origin = Camera.main.transform.position +
            Camera.main.transform.forward *
            Vector3.Distance(Camera.main.transform.position, startPoint.position);

        return origin;
    }
    /// <summary>
    /// GetOriginPos()를 통해 얻은 축에서 카메라의 Ray방향을 얻어낼 메소드
    /// </summary>
    /// <returns></returns>
    Vector3 CheckDir()
    {
        Vector3 dir = Vector3.zero;
        dir = Camera.main.transform.forward +
            Camera.main.transform.TransformDirection(dir);

        return dir;
    }*/

    /// <summary>
    /// 전달받은 Ray 위치에 PaintTarget.PaintRay() 실행
    /// <para>
    /// 이후 전달받은 _ammo값만큼 GunState에 소모값 요청
    /// </para>
    /// </summary>
    /// <param name="_ray"></param>
    /// <param name="_ammo"></param>
    void UsedAmmo(Ray _ray, int _ammo)
    {
        //GameObject effect = EffectManager.instance.GetEffect(EffectList.GunMuzzle);
        //effect.transform.position = startPoint.position;
        //effect.GetComponent<ParticleSystem>().Play();
        PaintTarget.PaintRay(_ray, brush, range);

        gun.UpdateState(_ammo);

        if (gun.Ammo <= 0)
        {
            gun.UpdateState(0, GunState.EMPTY);
        }
    }

    void CheckGizmo()
    {
        //Gizmos.color = Color.yellow;
        //Vector3 dir = Vector3.zero;
        //dir = CheckDir();     

        //Gizmos.DrawLine(GetOriginPos(), Camera.main.transform.forward);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(startPoint.position, Camera.main.transform.position + Camera.main.transform.forward * range);


        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
    }
}
