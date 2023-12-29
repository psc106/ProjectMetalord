using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GunStateController : MonoBehaviour
{
    enum CrossHair { ABLE, UNABLE}

    const int AmmoGaugeIdx = 2;
    const int WarningImgIdx = 4;

    GunBase[] mode = new GunBase[3];
    GunBase currentMode;
    public GunBase CurrentMode { get { return currentMode; } private set { currentMode = value; } }

    public void Shoot(GunMode _mode)
    {
        switch(_mode)
        {
            case GunMode.Paint:
                mode[(int)GunMode.Paint].ShootGun();
                break;
            case GunMode.Grab:                
                mode[(int)GunMode.Grab].ShootGun();
                break;
        }
    }

    public GunBase GetGunMode(int index)
    {
        return mode[index];
    }
    
    [SerializeField] private Controller_Physics player;
    [SerializeField] private Image crossHair;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject GunUi;
    [SerializeField] private Sprite[] crossHairSprite = new Sprite[2];

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
    
    public float Range { get { return range; } set { range = value; } }

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
    [HideInInspector] public static HashSet<PaintTarget> paintList = new HashSet<PaintTarget>();
    [HideInInspector] public static HashSet<SSC_BondObj> bondList = new HashSet<SSC_BondObj>();
    [HideInInspector] public static HashSet<NpcBase> npcList = new HashSet<NpcBase>();


    /*  { LEGACY : 기획변경에 따른 폐기
    // UI 제어 추가문
    public GameObject[] ModeUI = new GameObject[3];
    string[,] modeText = { { "벽타기", "1" }, { "당기기", "2" }, {"붙이기  ", "3" } };    
    TextMeshProUGUI[] currentText;
    TextMeshProUGUI[] elseText1;
    TextMeshProUGUI[] elseText2;

    */// } LEGACY : 기획변경에 따른 폐기

    Image[] gunImage;
    TextMeshProUGUI gunText;
    RectTransform textRect;
    Coroutine textFadeOut;

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
        set { maxAmmo = value; }
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
        gunImage = GunUi.transform.GetChild(0).GetComponentsInChildren<Image>();
        gunText = GunUi.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        textRect = gunText.GetComponent<RectTransform>();

        //currentText = ModeUI[0].GetComponentsInChildren<TextMeshProUGUI>();
        //elseText1 = ModeUI[1].GetComponentsInChildren<TextMeshProUGUI>();
        //elseText2 = ModeUI[2].GetComponentsInChildren<TextMeshProUGUI>();

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

        //ModeUI[1].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        //ModeUI[2].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        //레이캐스트 업데이트
        UpdateRaycast();

        if (onGrab || (reader.ShootKey && checkSuccessRay && CanFire && currentMode.CanFireAmmoCount()))
        {
            crossHair.color = Color.blue;
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
            float distanceCameraToHit = Vector3.Distance(startCameraPos, hit.point);
            float distancePlayerToHit = Vector3.Distance(startPlayerPos, hit.point);

            //카메라->끝점 range 이하 경우
            /*if (hit.distance <= distance)
            {
                checkSuccessRay = false;
                crossHair.color = Color.red;
                player.SetAimPosition(player.transform.position+Vector3.up*100);
                return;
            }

            else*/
            if (distanceCameraToHit <= cameraMinRange)
            {
                if (Physics.Raycast(defaultRay, out hit, distancePlayerToHit, gunLayer))
                {
                    player.SetAimPosition(hit.point+defaultRay.direction*10);
                    // Debug.Log("4");
                    // Debug.Log("플레이어->디폴트히트포인트");
                    startPoint = startPlayerPos;
                    checkSuccessRay = true;
                    minDistance = false;                    
                    crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;

                    if (!currentMode.CanFireAmmoCount() && !onGrab)
                    {
                        gunImage[WarningImgIdx].GetComponent<UiFadeOut>().InitFadeOut();
                        ChangedCrossHair();
                    }

                    //Vector3 anchor = Camera.main.WorldToScreenPoint(hit.point);
                    //if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)crossHair.transform.parent, anchor, null, out Vector2 localPoint))
                    {
                        //crossHair.rectTransform.anchoredPosition = localPoint;
                    }

                    return;
                }
            }


            //카메라->끝점 range 이상 경우
            else
            {
                player.SetAimPosition(hit.point + defaultRay.direction * 10);
                startPoint = startCameraPos;
                checkSuccessRay = true;
                minDistance = false;
                crossHair.color = CanFire && currentMode.CanFireAmmoCount() ? Color.green : Color.red;


                //if (CanFire && textFadeOut == null)
                //{                    
                //    textFadeOut = FadeOutText();
                //}

                crossHair.sprite = crossHairSprite[(int)CrossHair.ABLE];

                if (!currentMode.CanFireAmmoCount() && !onGrab)
                {
                    gunImage[WarningImgIdx].GetComponent<UiFadeOut>().InitFadeOut();
                    ChangedCrossHair();
                }

                //if (crossHair.rectTransform.anchoredPosition != Vector2.zero)
                {
                   // crossHair.rectTransform.anchoredPosition = Vector2.zero;
                }

                return;
            }
        }

        player.SetAimPosition(Vector3.zero);
        checkSuccessRay = false;

        if (!currentMode.CanFireAmmoCount() && !onGrab)
        {
            gunImage[WarningImgIdx].GetComponent<UiFadeOut>().InitFadeOut();
            ChangedCrossHair();
        }
        //else if (currentMode.CanFireAmmoCount())
        //{
        //    crossHair.sprite = crossHairSprite[(int)CrossHair.UNABLE];
        //    crossHair.color = Color.red;
        //}
        else
        {
            crossHair.color = Color.green;
            crossHair.sprite = crossHairSprite[(int)CrossHair.ABLE];
        }
        //if (CanFire && textFadeOut != null)
        //{
        //    StartCoroutine(textFadeOut);
        //}

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

    public void UpdateState(int ammoValue, GunState updatedState)
    {
        Ammo = ammoValue;
        state = updatedState;
        gunImage[AmmoGaugeIdx].fillAmount = (float)Ammo / (float)maxUpgrade;
    }

    public void UpdateState(int ammoValue)
    {        
        Ammo = ammoValue;
        gunImage[AmmoGaugeIdx].fillAmount = (float)Ammo / (float)maxUpgrade;        
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
        ClearAllPaint();
        currentMode.StopLerpGaguge();
        StopAllCoroutines();
        StartCoroutine(ReloadingAmmo());
    }

    IEnumerator ReloadingAmmo()
    {
        crossHair.gameObject.SetActive(false);

        InitColorText();

        textRect.position = crossHair.rectTransform.position;
        float currentAmmo = Ammo;

        float timeCheck = 0f;
        float t = 0f;

        while (timeCheck <= reloadTime)
        {
            timeCheck += Time.deltaTime;
            t = timeCheck / reloadTime;

            int ammoValue = (int)Mathf.Lerp(currentAmmo, maxAmmo, reloadCurve.Evaluate(t));
            UpdateState(ammoValue);
            UpdateText(timeCheck);

            yield return null;

        }

        checkAmmo = maxAmmo;
        UpdateState(maxAmmo, GunState.READY);
        gunText.text = "";
        crossHair.sprite = crossHairSprite[(int)CrossHair.ABLE];
        crossHair.gameObject.SetActive(true);
    }

    // 오버로딩 메소드 모음
    #region 오버로딩 메소드 모음
    public static void AddList(SSC_BondObj obj)
    {
        bondList.Add(obj);
    }

    public static void AddList(PaintTarget obj)
    {
        paintList.Add(obj);
    }

    public static void AddList(NpcBase obj)
    {
        npcList.Add(obj);
    }

    void ClearBondList()
    {
        foreach(var paint in bondList)
        {
            paint.CelarBond();
        }

        bondList.Clear();
    }

    void ClearNpcList()
    {
        foreach (var paint in npcList)
        {
            paint.ChangedState(npcState.normal);
        }

        bondList.Clear();
    }

    #endregion

    public void GunModeUnlock(GunMode gunMode)
    {
        switch (gunMode)
        {
            case GunMode.Paint:
                break;
            case GunMode.Grab:
                //ModeUI[1].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                //ModeUI[1].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                usedGrabGun = true;                
                break;
            case GunMode.Bond:
                //ModeUI[2].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                //ModeUI[2].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
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

        //SwapTest(changeMode);
        currentMode = mode[(int)changeMode];
        SwapLayer();
    }

    //void SwapTest(GunMode changeMode)
    //{        
    //    string[] tempString = new string[2];
    //    tempString[0] = currentText[0].text.Trim();
    //    tempString[1] = currentText[1].text.Trim();

    //    for (int i = 0; i < 3; i++)
    //    {
    //        if (ModeUI[i].transform.GetComponentInChildren<TextMeshProUGUI>().text.Trim() == modeText[(int)changeMode, 0].Trim())
    //        {
    //            ModeUI[i].transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text = tempString[0];
    //            ModeUI[i].transform.GetComponentsInChildren<TextMeshProUGUI>()[1].text = tempString[1];

    //            break;
    //        }        
    //    }

    //    for(int i = 0; i < 2; i++)
    //    {
    //        currentText[i].text = modeText[(int)changeMode, i];
    //    }        

    //    ModeUI[1].GetComponent<UiFadeOut>().InitFadeOut();
    //    ModeUI[2].GetComponent<UiFadeOut>().InitFadeOut();              
    //}

    public void ClearAllPaint()
    {
        foreach (PaintTarget target in paintList)
        {
            target.ClearPaint();
            target.splatTexPick = target.originTex;
        }

        paintList.Clear();
    }

    void UpdateText(float time)
    {
        if(time >=  reloadTime * 0.66f)
        {
            gunText.text = "충전중...";
        }
        else if(time >= reloadTime * 0.33f)
        {

            gunText.text = "충전중..";
        }
        else
        {
            gunText.text = "충전중.";
        }
    }

    public void ChangedCrossHair()
    {
        if(currentMode.CanFireAmmoCount())
        {
            crossHair.sprite = crossHairSprite[(int)CrossHair.ABLE];
        }
        else
        {
            InitColorText();
            Vector3 tempVec = crossHair.rectTransform.position;
            tempVec.y += 120f;
            textRect.position = tempVec;
            gunText.text = "접착제 부족!";
            crossHair.sprite = crossHairSprite[(int)CrossHair.UNABLE];
        }
    }

    IEnumerator IEFadeOutText()
    {
        Color tempColor = gunText.color;
        tempColor.a = 1f;
        gunText.color = tempColor;

        Vector3 tempVec = crossHair.rectTransform.position;
        tempVec.y += 120f;
        textRect.position = tempVec;        
        gunText.text = "너무 멀어!";

        float timeCheck = 0f;
        float fadeOutTime = 1f;
        float t = 0f;
        
        while(timeCheck < fadeOutTime)
        {
            timeCheck += Time.deltaTime;
            t = timeCheck / fadeOutTime;

            tempColor.a = Mathf.Lerp(1, 0, t);
            gunText.color = tempColor;

            yield return null;
        }        
    }

    void InitColorText()
    {
        Color tempColor = gunText.color;
        tempColor.a = 1f;
        gunText.color = tempColor;
    }

    // 
    public void CheckRangeCrossHair()
    {
        if(!checkSuccessRay)
        {
            if (textFadeOut != null) StopCoroutine(textFadeOut);
            textFadeOut = StartCoroutine(IEFadeOutText());            
            crossHair.color = Color.red;
            crossHair.sprite = crossHairSprite[(int)CrossHair.UNABLE];
        }
        else
        {
            crossHair.color = Color.green;
            crossHair.sprite = crossHairSprite[(int)CrossHair.ABLE];
        }
    }
}