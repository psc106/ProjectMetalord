using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
