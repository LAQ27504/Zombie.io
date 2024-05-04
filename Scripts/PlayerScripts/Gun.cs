using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }

    public event EventHandler<OnAmmoArgs> OnFiringAmmo;

    public event EventHandler<OnAmmoArgs> OnReloadDone;
    public class OnAmmoArgs : EventArgs
    {
        public int ammo;
    }


    [SerializeField] private Transform ammoPrefab;
    [SerializeField] private Player player;
    [SerializeField] private GameObject gunEffect;

    private float firingSpeed = 1;
    private float firingTimeCount;
    private float reloadTime = 3;
    private float reloadTimeCount = 0;
    private float range;
    private float blastTimer;
    public int ammo { get; private set; }
    public int ammoMax = 6;

    private float baseFiringSpeed = 1;
    private float baseReloadTime = 3;
    private int baseAmmoMax = 6;
    private float baseDamage = 10;


    private bool isReloading;

    private void Awake()
    {
        Instance = this;
        gunEffect.SetActive(false);
        ammo = ammoMax;
        player.OnFiringGun += Player_OnFiringGun;
        player.OnReloadGun += Player_OnReloadGun;
    }

    private void Player_OnReloadGun(object sender, System.EventArgs e)
    {
        player.isReload = true;
        isReloading = true;
        if (!IsReload())
        {
            reloadTimeCount = reloadTime; ;
        }
    }

    public void SetBack()
    {
        firingSpeed = baseFiringSpeed;
        reloadTime = baseReloadTime;
        ammoMax = baseAmmoMax;
        Ammo ammoGameObject = ammoPrefab.GetComponent<Ammo>();

        ammoGameObject.ChangeDamage(baseDamage);
    }

    public void SetUpGun()
    {
        ammo = ammoMax;
    }

    private void Player_OnFiringGun(object sender, Player.OnFiringGunArgs e)
    {
        if (IsDoneCooldown() && !IsReload() && IsHaveAmmo())
        {
            firingTimeCount = firingSpeed;

            Transform ammoTransfrom = Instantiate(ammoPrefab, e.firePointPosition, transform.rotation);

            Vector3 ammoDir = e.mousePosition - e.firePointPosition;

            this.range = CalRange(e.mousePosition, e.firePointPosition);

            blastTimer = 0.05f;

            gunEffect.SetActive(true);

            ammoTransfrom.GetComponent<Ammo>().Setup(ammoDir, this.range);

            CameraShake.Instance.ShakeCamera(5f, 0.1f);

            ammo--;

            OnFiringAmmo?.Invoke(this, new OnAmmoArgs
            {
                ammo = ammo
            });
        }
    }

    private void Update()
    {
        if (!IsDoneCooldown() && !IsReload())
        {
            FiringDown();
            BlastExist();
        }
        else if (IsReload())
        {
            ReloadCount();
        }
        else if (!IsReload())
        {
            Reload();
        }

    }

    private void BlastExist()
    {
        if (blastTimer > 0f)
        {
            blastTimer -= Time.deltaTime;
        }
        else
        {
            gunEffect.SetActive(false);
        }
    }

    private void FiringDown()
    {
        if (firingTimeCount > 0)
        {
            firingTimeCount -= Time.deltaTime;
        }
    }

    private void ReloadCount()
    {
        if (reloadTimeCount > 0)
        {
            reloadTimeCount -= Time.deltaTime;
        }
    }

    private void Reload()
    {
        if (isReloading)
        {
            ammo = ammoMax;
            player.isReload = false;
            OnReloadDone?.Invoke(this, new OnAmmoArgs
            {
                ammo = ammo
            });
            isReloading = false;
        }
    }

    private float CalRange(Vector3 mousePosition, Vector3 firePointPosition)
    {
        float range = MathF.Sqrt(MathF.Pow(mousePosition.x - firePointPosition.x, 2f) + MathF.Pow(mousePosition.y - firePointPosition.y, 2f));
        return range;
    }

    private bool IsDoneCooldown()
    {
        return firingTimeCount <= 0;
    }

    private bool IsHaveAmmo()
    {
        return ammo > 0;
    }

    private bool IsReload()
    {
        return reloadTimeCount > 0;
    }


    public float GetFireRate()
    {
        return firingSpeed;
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public int GetAmmo()
    {
        return ammoMax;
    }

    public float GetDamage()
    {
        return ammoPrefab.GetComponent<Ammo>().GetDamage();
    }

    public void ChangeFireRate(float fireRate)
    {
        firingSpeed = fireRate;
    }

    public void ChangeAmmo(int ammo)
    {
        ammoMax = ammo;
    }

    public void ChangeReload(float reloadTime)
    {
        this.reloadTime = reloadTime;
    }

    public void ChangeDamage(float damage)
    {
        Ammo ammoGameObject = ammoPrefab.GetComponent<Ammo>();

        ammoGameObject.ChangeDamage(damage);
    }
}
