using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public enum GunState_TODO
{
    READY,
    EMPTY,
    RELOADING
}

public class GunStateController : MonoBehaviour
{
    GunBase[] mode = new GunBase[3];
    public GunBase currentMode;

    [SerializeField] private TextMeshProUGUI AmmoText;
    [SerializeField] private Image AmmoGauge;
    [SerializeField] private Controller_Physics player;
    [SerializeField] private AnimationCurve reloadCurve;
    [SerializeField] private Image crossHair;
    [SerializeField] private Transform crossHairRect;

    public InputReader reader;    
    public Transform checkPos;
    public Transform pickupPoint;
    public Transform GunHolderHand;
    public LineRenderer grabLine;

    bool usedGrabGun = false;
    bool usedBondGun = false;

    [SerializeField]
    private Transform startPos;

    public Transform AimTarget;

    [Range(0, 100)]
    public float range;
    [SerializeField, Range(0, 10)]
    float minRange;
    [SerializeField, Range(0, 10)]
    float reloadTime = 4.5f;

    public LayerMask gunLayer;

    [HideInInspector]
    public Vector3 startPoint;
    [HideInInspector]
    public RaycastHit hit;

    [HideInInspector]
    public bool checkSuccessRay = false;
    [HideInInspector] 
    public bool onGrab = false;
    [HideInInspector]
    public bool minDistance = false;

    [HideInInspector] public NpcBase targetNpc = null;
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
            if (state != GunState.READY)
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
            if (onGrab)
            {
                return !onGrab;
            }

            return player.CanReload;
        }
        private set { }
    }

    int maxAmmo = 200;
    public int MaxAmmo
    {
        get { return maxAmmo; }
        private set { maxAmmo = value; }
    }

    private int ammo;
    public int Ammo
    {
        get { return ammo; }
        private set { ammo = value; }
    }

    private void Awake()
    {
        mode[(int)GunMode.Paint] = transform.GetComponent<PaintGun>();
        mode[(int)GunMode.Grab] = transform.GetComponent<GrabGun>();
        mode[(int)GunMode.Bond] = transform.GetComponent<BondGun>();

        for (int i = 0; i < mode.Length; i++)
        {
            mode[i].hideFlags = HideFlags.HideInInspector;
        }

        currentMode = mode[(int)GunMode.Paint];
        SwapLayer();

    }

    private void OnEnable()
    {
        GameEventsManager.instance.coinEvents.onUnlockGunMode += GunModeUnlock;
        GameEventsManager.instance.coinEvents.onUpgradeGun += UpgradeGun;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.coinEvents.onUnlockGunMode -= GunModeUnlock;
        GameEventsManager.instance.coinEvents.onUpgradeGun -= UpgradeGun;
    }

    void Start()
    {
        Ammo = MaxAmmo;
        AmmoText.text = MaxAmmo + " / " + Ammo;

        state = GunState.READY;
    }

    void SwapLayer()
    {
        gunLayer = currentMode.myLayer;
    }


    private void Update()
    {
        //레이캐스트 업데이트
        UpdateRaycast();
        if (onGrab || (reader.ShootKey && checkSuccessRay && CanFire && currentMode.CanFireAmmoCount()))
        {
            crossHair.color = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log($"현재 레인지 값 : {range}");
            Debug.Log($"현재 총량 :  { MaxAmmo}");
            //Debug.Log($"본드 리스트 수 : {bondList.Count}");
        }
    }

    private void UpdateRaycast()
    {
        Vector3 startCameraPos = GetOriginPos();
        Vector3 startPlayerPos = checkPos.position;
        Vector3 direction = CheckDir();

        Vector3 endPos = startCameraPos + Camera.main.transform.forward * range;

        Ray normalRay = new Ray(startCameraPos, direction);
        Ray checkRay = new Ray(startPlayerPos, direction);
        Ray defaultRay = new Ray(startPlayerPos, (endPos - startPlayerPos).normalized);

        float distance = Vector3.Distance(Camera.main.transform.position, startPos.position);

        // 일반적인 상황의 사격
        if (Physics.Raycast(normalRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(startCameraPos, hit.point);

            //카메라->끝점 range 이하 경우
            if (checkDistance <= minRange)
            {
               /* if (Physics.Raycast(defaultRay, out hit, range, gunLayer))
                {
                    //Debug.Log("플레이어->디폴트히트포인트");
                    startPoint = startPlayerPos;
                    checkSuccessRay = true;
                    minDistance = true;
                    crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

                    Vector3 anchor = Camera.main.WorldToScreenPoint(hit.point);
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)crossHair.transform.parent, anchor, null, out Vector2 localPoint))
                    {
                        crossHair.rectTransform.anchoredPosition = localPoint;
                    }
                    return;
                }*/
            }

            //카메라->끝점 range 이상 경우
            else
            {
                //minDistance = false;    
                //Debug.Log($"일정거리 이하 : {minDistance}");
                //AimTarget.position = hit.point;
                startPoint = startCameraPos;
                checkSuccessRay = true;
                minDistance = false;
                crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

                if (crossHair.rectTransform.anchoredPosition != Vector2.zero)
                {
                    crossHair.rectTransform.anchoredPosition = Vector2.zero;
                }
                return;
            }
        }

        // 정면 오브젝트와 설정한 rangeLimit 값 거리 이하일 때
        if (Physics.Raycast(defaultRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(startPlayerPos, hit.point);

            //Debug.Log("플레이어->디폴트히트포인트");
            startPoint = startPlayerPos;
            checkSuccessRay = true;
            minDistance = false;
            crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

            Vector3 anchor = Camera.main.WorldToScreenPoint(hit.point);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)crossHair.transform.parent, anchor, null, out Vector2 localPoint))
            {
                crossHair.rectTransform.anchoredPosition = localPoint;
            }
            return;

            //플레이어->끝점 range 이하 경우
            if (checkDistance <= minRange)
            {
                /*Debug.Log("플레이어->플레이어정면");
                minDistance = true;
                //Debug.Log($"일정거리 이하 : {minDistance}");
                startPoint = startPlayerPos;
                //AimTarget.position = hit.point;
                checkSuccessRay = true;
                crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

                Vector3 anchor = Camera.main.WorldToScreenPoint(hit.point);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)crossHair.transform.parent, anchor, null, out Vector2 localPoint))
                {
                    crossHair.rectTransform.anchoredPosition = localPoint;
                }
                return;*/
            }

            //플레이어->끝점 range 이상 경우
            else
            {
                /*if (Physics.Raycast(defaultRay, out hit, range, gunLayer))
                {
                    Debug.Log("플레이어->디폴트히트포인트");
                    startPoint = startPlayerPos;
                    checkSuccessRay = true;
                    minDistance = false;
                    crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

                    Vector3 anchor = Camera.main.WorldToScreenPoint(hit.point);
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)crossHair.transform.parent, anchor, null, out Vector2 localPoint))
                    {
                        crossHair.rectTransform.anchoredPosition = localPoint;
                    }
                    return;
                }*/
            }
        }
       


        if (crossHair.rectTransform.anchoredPosition != Vector2.zero)
        {
            crossHair.rectTransform.anchoredPosition = Vector2.zero;
        }
        checkSuccessRay = false;
        crossHair.color = Color.red;

    }

    /// <summary>
    /// 카메라와 플레이어의 축을 동일선상에 놓아주는 메소드
    /// </summary>
    /// <returns>카메라의 정면 방향 + 플레이어의 수직선상 </returns>
    Vector3 GetOriginPos()
    {
        Vector3 origin = Vector3.zero;

        origin = Camera.main.transform.position 
            //+ Camera.main.transform.forward 
            //* Vector3.Distance(Camera.main.transform.position, startPos.position)
            ;

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
        if(state == GunState.RELOADING || onGrab)
        {
            return;
        }

        currentMode = mode[(int)GunMode.Paint];
        SwapLayer();

    }

    public void SwapGrabGun()
    {
        if (state == GunState.RELOADING || !usedGrabGun)
        {
            return;
        }

        currentMode = mode[(int)GunMode.Grab];        
        SwapLayer();
    }

    public void SwapBondGun()
    {
        if (state == GunState.RELOADING || onGrab || !usedBondGun)
        {
            return;
        }

        currentMode = mode[(int)GunMode.Bond];        
        SwapLayer();
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
        float t = 0f;

        while (timeCheck <= reloadTime)
        {
            t = timeCheck / reloadTime;

            int ammoValue = (int)Mathf.Lerp(Ammo, maxAmmo, reloadCurve.Evaluate(t));
            UpdateState(ammoValue - Ammo);

            yield return null;
            timeCheck += Time.deltaTime;

        }

        UpdateState(maxAmmo, GunState.READY);
    }

    public static void AddList(SSC_BondObj obj)
    {
        for (int i = 0; i < bondList.Count; i++)
        {
            if (bondList[i] == obj)
            {
                return;
            }
        }

        bondList.Add(obj);
    }

    public static void AddList(PaintTarget obj)
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

    public static void AddList(NpcBase obj)
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

    void ClearBondList()
    {
        for (int i = 0; i < bondList.Count; i++)
        {
            bondList[i].CelarBond();
        }

        bondList.Clear();
    }

    void ClearNpcList()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].ChangedState(npcState.normal);
        }

        bondList.Clear();
    }

    public void GunModeUnlock(GunMode gunMode)
    {
        switch (gunMode)
        {
            case GunMode.Paint:
                break;
            case GunMode.Grab:
                usedGrabGun = true;
                break;
            case GunMode.Bond:
                usedBondGun = true;
                break;
        }
    }

    public void UpgradeGun(UpgradeCategory _category, int _value)
    {
        switch (_category)
        {
            case UpgradeCategory.Range:
                range += _value;
                break;
            case UpgradeCategory.Amount:
                MaxAmmo += _value;
                break;
        }
    }
}