using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Gun playerGun;

    private float fireRate;

    private float reloadTime;

    private int ammo;

    private float damage;

    private void Awake()
    {
        fireRate = playerGun.GetFireRate();

        reloadTime = playerGun.GetReloadTime();

        ammo = playerGun.GetAmmo();

        damage = playerGun.GetDamage();
    }

    public void SetUp()
    {
        DamageUI.Instance.OnDamageUpgraded += Instance_OnDamageUpgraded;
        AmmoUpgradeUI.Instance.OnAmmoUpgraded += Instance_OnAmmoUpgraded;
        FireRateUI.Instance.OnFireRateUpgraded += Instance_OnFireRateUpgraded;
        ReloadUI.Instance.OnReloadUpgraded += Instance_OnReloadUpgraded;
    }

    public void SetDown()
    {
        DamageUI.Instance.OnDamageUpgraded -= Instance_OnDamageUpgraded;
        AmmoUpgradeUI.Instance.OnAmmoUpgraded -= Instance_OnAmmoUpgraded;
        FireRateUI.Instance.OnFireRateUpgraded -= Instance_OnFireRateUpgraded;
        ReloadUI.Instance.OnReloadUpgraded -= Instance_OnReloadUpgraded;
    }

    private void Instance_OnReloadUpgraded(object sender, System.EventArgs e)
    {
        reloadTime -= 0.4f;

        playerGun.ChangeReload(reloadTime);

        Debug.Log("reload Upgrade");
    }

    private void Instance_OnFireRateUpgraded(object sender, System.EventArgs e)
    {
        fireRate -= 0.15f;

        playerGun.ChangeFireRate(fireRate);

        Debug.Log("Fire rate Upgrade");
    }

    private void Instance_OnAmmoUpgraded(object sender, System.EventArgs e)
    {
        ammo += 1;

        playerGun.ChangeAmmo(ammo);

        Debug.Log("ammo upgraded");
    }

    private void Instance_OnDamageUpgraded(object sender, System.EventArgs e)
    {
        damage += 10f;

        playerGun.ChangeDamage(damage);

        Debug.Log("Damage Upgrade");
    }
}
