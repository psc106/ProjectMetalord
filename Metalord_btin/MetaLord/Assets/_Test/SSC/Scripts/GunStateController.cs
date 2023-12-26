using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GunStateController : MonoBehaviour
{
    GunBase[] mode = new GunBase[3];
    GunBase currentMode;
    public GunBase CurrentMode { get { return currentMode; } private set { currentMode = value; } }
    
    [SerializeField] private Controller_Physics player;
    [SerializeField] private Image crossHair;
    [SerializeField] private Transform startPos;

    public Rigidbody getconnect()
    {
        return player.connectedBody;
    }

    public InputReader reader;    
    public Transform checkPos;
    public Transform pickupPoint;
    public Transform GunHolderHand;
    public LineRenderer grabLine;
    public Transform AimTarget;
    public LayerMask gunLayer;

    [Header("도구 UI")]        
    [SerializeField] private Image AmmoGauge;
    [SerializeField] private Color[] ModeColor = new Color[3];
    public int CloseUp_FontSize = 32;
    public int CloseOff_FontSize = 27;
    [SerializeField] private AnimationCurve reloadCurve;

    [Header("도구 스텟")]
    [Range(1, 50)]
    public float ClimbeSize = 10f;
    [Range(1, 50)]
    public float BondSize = 20f;
    [Range(0, 500)]
    public float range; 
    [SerializeField, Range(0, 10)]
    float cameraMinRange;
    [SerializeField, Range(0, 10)]
    float playerMinRange;
    [SerializeField, Range(0,500)]
    float grabRange = 20f;
    public float GrabRange { get { return grabRange; } private set { grabRange = GrabRange; } }
    [SerializeField, Range(0, 10)]
    float reloadTime = 3f;
    [Range(0.1f, 3f)]
    public float AutoInitTime = 1f;
    [Range(0.01f, 1)]
    public float fireRate = 0.1f;
    [SerializeField, Range(0, 1000)]
    private int maxAmmo = 200;    

    [HideInInspector]
    public Vector3 startPoint;
    [HideInInspector]
    public RaycastHit hit;
    [HideInInspector]
    public float lerpTime= .1f;
    [HideInInspector]
    public int checkAmmo;
    

    [HideInInspector]
    public bool checkSuccessRay = false;
    [HideInInspector] 
    public bool onGrab = false;
    [HideInInspector]
    public bool minDistance = false;

    bool usedGrabGun = false;
    bool usedBondGun = false;
    int maxUpgrade = 405;

    [HideInInspector] public NpcBase targetNpc = null;
    [HideInInspector] public GunState state;
    [HideInInspector] public static List<PaintTarget> paintList = new List<PaintTarget>();
    [HideInInspector] public static List<SSC_BondObj> bondList = new List<SSC_BondObj>();
    [HideInInspector] public static List<NpcBase> npcList = new List<NpcBase>();


    // UI 제어 추가문
    public GameObject[] ModeUI = new GameObject[3];
    string[,] modeText = { { "벽타기", "1" }, { "당기기", "2" }, {"붙이기  ", "3" } };    
    TextMeshProUGUI[] currentText;
    TextMeshProUGUI[] elseText1;
    TextMeshProUGUI[] elseText2;

    // 프로퍼티 모음
    #region Property
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

    #endregion

    // CallBack 함수
    #region CallBack
    private void Awake()
    {
        lerpTime = Time.deltaTime;

        currentText = ModeUI[0].GetComponentsInChildren<TextMeshProUGUI>();
        elseText1 = ModeUI[1].GetComponentsInChildren<TextMeshProUGUI>();
        elseText2 = ModeUI[2].GetComponentsInChildren<TextMeshProUGUI>();

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

    void Start()
    {
        UpdateState(MaxAmmo, GunState.READY);
        checkAmmo = Ammo;

        ModeUI[1].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        ModeUI[2].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
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
            Debug.Log($"본드 리스트 수 : {bondList.Count}");
        }
    }

    private void OnDisable()
    {
        GameEventsManager.instance.coinEvents.onUnlockGunMode -= GunModeUnlock;
        GameEventsManager.instance.coinEvents.onUpgradeGun -= UpgradeGun;
    }
    #endregion

    void SwapLayer()
    {
        gunLayer = currentMode.myLayer;
    }
    
    private void UpdateRaycast()
    {
        Vector3 startCameraPos = GetOriginPos();
        Vector3 startPlayerPos = checkPos.position;
        Vector3 direction = CheckDir();

        Vector3 endPos = startCameraPos + Camera.main.transform.forward * range;

        Ray normalRay = new Ray(Camera.main.transform.position, direction);
        Ray checkRay = new Ray(startPlayerPos, direction);
        Ray defaultRay = new Ray(startPlayerPos, (endPos - startPlayerPos).normalized);

        float distance = Vector3.Distance(Camera.main.transform.position, GetOriginPos());

        
        // 일반적인 상황의 사격
        if (Physics.Raycast(normalRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(startCameraPos, hit.point);

            //카메라->끝점 range 이하 경우
            if(hit.distance <= distance)
            {
                //Debug.Log("0");

            }
            else if (checkDistance <= cameraMinRange)
            {
                // 정면 오브젝트와 설정한 rangeLimit 값 거리 이하일 때
                if (Physics.Raycast(checkRay, out hit, range, gunLayer))
                {
                    checkDistance = Vector3.Distance(startPlayerPos, hit.point);
                    //Debug.Log(checkDistance);
                    //플레이어->끝점 range 이하 경우
                    if (checkDistance <= playerMinRange)
                    {
                        //Debug.Log("3");
                        //Debug.Log("플레이어->플레이어정면");
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
                        return;
                    }

                    //플레이어->끝점 range 이상 경우
                    else
                    {
                        if (Physics.Raycast(defaultRay, out hit, range * 0.6f, gunLayer))
                        {
                            //Debug.Log("4");
                            // Debug.Log("플레이어->디폴트히트포인트");
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
                        }
                    }
                }
            }

            //카메라->끝점 range 이상 경우
            else
            {
                //Debug.Log("2");
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
            + Camera.main.transform.forward 
            * Vector3.Distance(Camera.main.transform.position, startPos.position)
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
        AmmoGauge.fillAmount = (float)Ammo / (float)maxUpgrade;
    }

    public void UpdateState(int ammoValue)
    {        
        Ammo = ammoValue;                
        AmmoGauge.fillAmount = (float)Ammo / (float)maxUpgrade;
    }

    public void Reloading()
    {
        if (!CanReload || state == GunState.RELOADING || Ammo == MaxAmmo)
        {
            return;
        }

        player.PlayReloadAnimation();
        int id = (int)GunSoundList.Reload;
        SoundManager.instance.PlaySound(GroupList.Gun, id);

        state = GunState.RELOADING;
        ClearBondList();
        ClearNpcList();
        PaintTarget.ClearAllPaint();
        currentMode.StopLerpGaguge();
        StartCoroutine(ReloadingAmmo());
    }

    IEnumerator ReloadingAmmo()
    {
        float currentAmmo = Ammo;

        float timeCheck = 0f;
        float t = 0f;

        while (timeCheck <= reloadTime)
        {
            timeCheck += Time.deltaTime;
            t = timeCheck / reloadTime;

            int ammoValue = (int)Mathf.Lerp(currentAmmo, maxAmmo, reloadCurve.Evaluate(t));
            UpdateState(ammoValue);

            yield return null;

        }

        checkAmmo = maxAmmo;
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
                ModeUI[1].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                ModeUI[1].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                usedGrabGun = true;                
                break;
            case GunMode.Bond:
                ModeUI[2].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                ModeUI[2].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
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
                checkAmmo += _value;
                MaxAmmo += _value;                
                UpdateState(Ammo + _value);
                break;
        }
    }

    public void SwapGunMode(GunMode changeMode)
    {
        // 각종 모드 스왑을 막아야 하는 상황 1. 리로딩중, 그랩모드중 그랩일때, 이미 내가 활성화한 모드일때
        if (state == GunState.RELOADING || onGrab || currentMode == mode[(int)changeMode])
        {
            return;
        }

        // 해금모드들 해금전에 스왑 방지
        if((changeMode == GunMode.Grab && !usedGrabGun) ||
            (changeMode == GunMode.Bond && !usedBondGun))
        {
            return;
        }

        int id = (int)GunSoundList.ChangeMod;
        SoundManager.instance.PlaySound(GroupList.Gun, id);

        SwapTest(changeMode);
        currentMode = mode[(int)changeMode];
        SwapLayer();
    }

    void SwapTest(GunMode changeMode)
    {        
        string[] tempString = new string[2];
        tempString[0] = currentText[0].text.Trim();
        tempString[1] = currentText[1].text.Trim();

        for (int i = 0; i < 3; i++)
        {
            if (ModeUI[i].transform.GetComponentInChildren<TextMeshProUGUI>().text.Trim() == modeText[(int)changeMode, 0].Trim())
            {
                ModeUI[i].transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text = tempString[0];
                ModeUI[i].transform.GetComponentsInChildren<TextMeshProUGUI>()[1].text = tempString[1];

                break;
            }        
        }

        for(int i = 0; i < 2; i++)
        {
            currentText[i].text = modeText[(int)changeMode, i];
        }        

        ModeUI[1].GetComponent<UiFadeOut>().InitFadeOut();
        ModeUI[2].GetComponent<UiFadeOut>().InitFadeOut();              
    }

}