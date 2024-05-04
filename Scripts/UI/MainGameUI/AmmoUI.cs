using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    
    private Gun gun;

    private int ammoIndex;
    private int presentIndex;

    private void Awake()
    {

    }
    public void GetGun(Gun gun)
    {
        this.gun = gun;

        this.gun.OnFiringAmmo += Gun_OnFiringAmmo;
        this.gun.OnReloadDone += Gun_OnReloadDone;
        InitVisual(this.gun.ammoMax);
        ammoIndex = this.gun.ammoMax;
    }

    

    private void Gun_OnReloadDone(object sender, Gun.OnAmmoArgs e)
    {
        UpdateVisual(e.ammo);
    }

    private void Gun_OnFiringAmmo(object sender, Gun.OnAmmoArgs e)
    {
        UpdateVisual(e.ammo);
    }

    private void InitVisual(int ammo)
    {
        ammoCount.text = "x" + ammo.ToString();
    }
    private void UpdateVisual(int ammo)
    {
        ammoCount.text = "x" + ammo.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
