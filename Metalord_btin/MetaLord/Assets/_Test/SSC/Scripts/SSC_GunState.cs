using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public enum GunState
{
    READY,
    EMPTY,
    RELOADING
}

public class SSC_GunState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AmmoText;
    [SerializeField] private Image AmmoGauge;
    [SerializeField] private SSC_PaintGun paintMode;
    [SerializeField] private SSC_GrabGun grabMode;
    [SerializeField] private SSC_BondGun bondMode;
    [SerializeField] private Controller_Physics player;
    [SerializeField] private MeshRenderer gunRenderer;
    [SerializeField] private GameObject backGun;
    [SerializeField] private AnimationCurve reloadCurve;

    float reloadTime = 4.5f;
    [HideInInspector] public GunState state;
    [HideInInspector] public static List<PaintTarget> paintList = new List<PaintTarget>();
    [HideInInspector] public static List<SSC_BondObj> bondList = new List<SSC_BondObj>();
    [HideInInspector] public static List<NpcBase> npcList = new List<NpcBase>();



    //public Vector3 GetPlayerCenter()
    //{
    //    return player.GetPlayerCenter();
    //}

    public bool CanFire
    {
        get 
        {
            if(state != GunState.READY)
            {
                return false;
            }

            return player.CanFire;
        }
        private set { }
    }

    public bool CanReload
    {
        get 
        { 
            if(grabMode.OnGrab)
            {
                return !grabMode.OnGrab;
            }

            return !player.OnClimb; 
        }
        private set { }
    }

    int maxAmmo = 200;
    public int MaxAmmo
    {
        get {  return maxAmmo; } 
        private set { maxAmmo = value; }
    }

    private int ammo;
    public int Ammo
    {
        get { return ammo; }
        private set { ammo = value; }
    }
    
    void Start()
    {
        grabMode.enabled = false;
        bondMode.enabled = false;
        paintMode.enabled = true;

        Ammo = MaxAmmo;
        AmmoText.text = MaxAmmo + " / " + Ammo;

        state = GunState.READY;
    }

    private void Update()
    {
        //레이캐스트 업데이트
        UpdateRaycast();

        if(Input.GetKeyDown(KeyCode.L))
        {
            if(hit.transform.GetComponent<NpcBase>() != null)
            {
                hit.transform.GetComponent<NpcBase>().PrintState();
            }
        }

        // 장전        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }

        // 1번키 누르면 페인트건
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {            
            SwapPaintGun();
        }

        // 2번키 누르면 그랩건
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapGrabGun();
        }

        // 3번키 누르면 본드건
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwapBondGun();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log($"본드 리스트 수 : {bondList.Count}");
        }
    }

    [SerializeField]
    private Transform checkPos;
    [SerializeField]
    private Transform startPos;

    [SerializeField, Range(0,100)]
    float range;
    [SerializeField, Range(0, 10)]
    float minRange;

    [SerializeField]
    LayerMask gunLayer;

    [HideInInspector]
    public Vector3 startPoint;
    [HideInInspector]
    public RaycastHit hit;
    [HideInInspector]
    public bool checkSuccessRay = false;
    
    private void UpdateRaycast()
    {
        Vector3 startCameraPos = GetOriginPos();
        Vector3 startPlayerPos = checkPos.position;

        Ray normalRay = new Ray(startCameraPos, CheckDir());
        Ray checkRay = new Ray(startPlayerPos, CheckDir());

        // 일반적인 상황의 사격
        if (Physics.Raycast(normalRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(startCameraPos, hit.point);

            //카메라->끝점 range 이하 경우
            if (checkDistance <= minRange)
            {
                //something
            }

            //카메라->끝점 range 이상 경우
            else
            {
                startPoint = startCameraPos;
                checkSuccessRay = true;
                return;
            }
        }

        // 정면 오브젝트와 설정한 rangeLimit 값 거리 이하일 때
        else if (Physics.Raycast(checkRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(startPlayerPos, hit.point);

            //카메라->끝점 range 이하 경우
            if (checkDistance <= minRange)
            {
                startPoint = startPlayerPos;
                checkSuccessRay = true;
                return;
            }

            //카메라->끝점 range 이상 경우
            else
            {
                //something
            }
        }

        checkSuccessRay = false;

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
            Vector3.Distance(Camera.main.transform.position, startPos.position);

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

    public void SwapPaintGun()
    {
        if (grabMode.OnGrab || state == GunState.RELOADING)
        {
            return;
        }

        grabMode.enabled = false;
        bondMode.enabled = false;
        paintMode.enabled = true;
    }

    private void SwapGrabGun()
    {
        if (state == GunState.RELOADING)
        {
            return;
        }

        paintMode.enabled = false;
        bondMode.enabled = false;
        grabMode.enabled = true;
    }

    public void SwapBondGun()
    {
        if (grabMode.OnGrab || state == GunState.RELOADING)
        {
            return;
        }

        paintMode.enabled = false;
        grabMode.enabled = false;
        bondMode.enabled = true;
    }

    public void UpdateState(int ammoValue, GunState updatedState)
    {
        Ammo = ammoValue;
        state = updatedState;
        AmmoText.text = MaxAmmo + " / " + Ammo;
        AmmoGauge.fillAmount = (float)Ammo / (float)MaxAmmo;
    }

    public void UpdateState(int ammoValue)
    {
        Ammo += ammoValue;
        AmmoText.text = MaxAmmo + " / " + Ammo;
        AmmoGauge.fillAmount = (float)Ammo / (float)MaxAmmo;
    }

    public void Reloading()
    {
        if (!CanReload || state == GunState.RELOADING)
        {
            return;
        }

        player.PlayReloadAnimation();
        state = GunState.RELOADING;
        ClearBondList();
        ClearNpcList();
        PaintTarget.ClearAllPaint();
        StartCoroutine(ReloadingAmmo());
    }

    IEnumerator ReloadingAmmo()
    {
        float timeCheck = 0f;
        float x = 0f;

        while (timeCheck <= reloadTime)
        {
            x = timeCheck / reloadTime;                        

            int ammoValue = (int)Mathf.Lerp(Ammo, maxAmmo, x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2);            
            UpdateState(ammoValue - Ammo);  
            
            yield return null;
            timeCheck += Time.deltaTime;

        }

        UpdateState(maxAmmo, GunState.READY);
    }

    public static void AddBondList(SSC_BondObj obj)
    {        
        for(int i = 0; i < bondList.Count; i++)
        {
            if (bondList[i] == obj)
            {
                return;
            }
        }

        bondList.Add(obj);
    }

    public static void AddBondList(NpcBase obj)
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            if (npcList[i] == obj)
            {
                return;
            }
        }

        npcList.Add(obj);
    }

    void ClearNpcList()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].ChangedState(npcState.normal);
        }

        npcList.Clear();
    }

    public static void AddPaintList(PaintTarget obj)
    {
        for (int i = 0; i < paintList.Count; i++)
        {
            if (paintList[i] == obj)
            {
                return;
            }
        }

        paintList.Add(obj);
    }

    void ClearBondList()
    {        
        for(int i = 0; i < bondList.Count; i++)
        {
            bondList[i].CelarBond();
        }

        bondList.Clear();
    }

}
