using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GunState
{
    READY,
    EMPTY,
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

    [HideInInspector] public GunState state;

    public Vector3 GetPlayerCenter()
    {
        return player.GetPlayerCenter();
    }

    public bool CanFire
    {
        get { return player.CanFire; }
        private set { }
    }

    public bool CanReload
    {
        get { return !player.OnClimb; }
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
        if (CanFire)
        {
            gunRenderer.enabled = true;
            backGun.SetActive(false);
        }
        else
        {
            gunRenderer.enabled = false;
            backGun.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            grabMode.enabled = false;
            paintMode.enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            paintMode.enabled = false;
            grabMode.enabled = true;
        }
    }

    public void UpdateState(int ammoValue, GunState updatedState)
    {
        ammo = ammoValue;
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

}
