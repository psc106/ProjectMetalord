using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SSC_PaintGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform checkPos;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform aimTarget;


    [SerializeField] private LayerMask gunLayer = -1;
    [Range(0.1f, 1f)] public float attackSpeed;

    [SerializeField, Range(0, 1)]
    float autoTime = 1f;
    [SerializeField, Range(1, 100)]
    float range = 50f;

    float rangeLimit = 4f;

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

    private void OnDrawGizmos()
    {
        //CheckGizmo();
    }

    // Update is called once per frame
    void Update()
    {
        Ray normalRay = new Ray(GetOriginPos(), CheckDir());
        Ray checkRay = new Ray(checkPos.position, CheckDir());

        // 레이지점 컬러 체크 테스트용
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(PaintTarget.CursorColor());
        }

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
        else if(autotimeCheck > autoTime && gun.CanFire)
        {
            AutoFire(normalRay, checkRay);
            return;
        }

        // 사격을 시작 == 마우스버튼 누른시점동안
        else if(fireStart == true)
        {
            autotimeCheck += Time.deltaTime;
            return;
        }

        else if(input.ShootKey && gun.CanFire)
        {            
            NormalFire(normalRay, checkRay);
        }

    }

    /// <summary>
    /// 단발 메소드
    /// </summary>
    private void NormalFire(Ray normalRay, Ray checkRay)
    {     
        RaycastHit hit;

        // 정면 오브젝트와 설정한 rangeLimit 값 거리 이하일 때
        if(Physics.Raycast(checkRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(checkPos.position, hit.point);

            if (checkDistance <= rangeLimit)
            {
                UsedAmmo(checkRay, normalShot);

                fireStart = true;
                return;
            }
        }

        // 일반적인 상황의 사격
        if (Physics.Raycast(normalRay, out hit, range, gunLayer))
        {
            Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

            UsedAmmo(muzzleRay, normalShot);

            fireStart = true;
        }
        
    }

    /// <summary>
    /// 연사 메소드
    /// </summary>
    private void AutoFire(Ray normalRay, Ray checkRay)
    {
        timeCheck += Time.deltaTime;
        RaycastHit hit;

        if (timeCheck >= attackSpeed)
        {
            if (Physics.Raycast(checkRay, out hit, range, gunLayer))
            {
                float checkDistance = Vector3.Distance(checkPos.position, hit.point);

                if (checkDistance <= rangeLimit)
                {
                    UsedAmmo(checkRay, autoShot);
                    
                    timeCheck = 0f;
                    return;
                }                                
            }
        }

        if (timeCheck >= attackSpeed)
        {                        
            if (Physics.Raycast(normalRay, out hit, range, gunLayer))
            {
                Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

                UsedAmmo(muzzleRay, autoShot);

                timeCheck = 0f;        
            }            
        }
    }

    /// <summary>
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
    }

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
