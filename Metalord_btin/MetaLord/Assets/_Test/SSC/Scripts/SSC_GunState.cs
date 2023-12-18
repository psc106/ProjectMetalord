using System;
using System.Collections;
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
    [SerializeField] private Controller_Physics player;
    [SerializeField] private MeshRenderer gunRenderer;
    [SerializeField] private GameObject backGun;
    [SerializeField] private AnimationCurve reloadCurve;

    float reloadTime = 4.5f;
    [HideInInspector] public GunState state;

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
        paintMode.enabled = true;

        Ammo = MaxAmmo;
        AmmoText.text = MaxAmmo + " / " + Ammo;

        state = GunState.READY;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) && CanReload && state != GunState.RELOADING)
        {
            player.PlayReloadAnimation();
            state = GunState.RELOADING;
            PaintTarget.ClearAllPaint();
            StartCoroutine(ReloadingAmmo());
            //UpdateState(MaxAmmo, GunState.READY);
        }

        if (Input.GetKeyDown(KeyCode.Q) &&
            !grabMode.OnGrab
            && state != GunState.RELOADING)
        {            
            grabMode.enabled = false;
            paintMode.enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.E) &&
            state != GunState.RELOADING)
        {            
            paintMode.enabled = false;
            grabMode.enabled = true;
        }
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

}
