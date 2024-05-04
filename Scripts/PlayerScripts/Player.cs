using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IAbsorbDamage
{
    public static Player Instance;

    public event EventHandler OnDead;
    public event EventHandler<OnFiringGunArgs> OnFiringGun;
    public event EventHandler OnReloadGun;

    public class OnFiringGunArgs : EventArgs
    {
        public Vector3 mousePosition;
        public Vector3 firePointPosition;
    }

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;

    private Vector2 inputVector;

    private float moveSpeed;
    private float health;
    private float healthMax = 100f;
    private float energy;
    private float energyMax = 100f;
    private float rest;
    private float restMax = 3f;
    
    private bool canRun;
    private bool isEquiped;
    public bool isReload { get; set; }

    public bool canRotate { get; set; } 

    private Vector3 mousePosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerMovement.OnFiring += PlayerMovement_OnFiring;
        playerMovement.OnEquipedWeapon += PlayerMovement_OnEquipedWeapon;
        playerMovement.OnReload += PlayerMovement_OnReload;

        isEquiped = false;
    }

    private void PlayerMovement_OnReload(object sender, EventArgs e)
    {
        OnReloadGun?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerMovement_OnEquipedWeapon(object sender, PlayerMovement.OnEquipedWeaponArgs e)
    {
        isEquiped = e.isEquiped;
    }

    private void PlayerMovement_OnFiring(object sender, System.EventArgs e)
    {
        OnFiringGun?.Invoke(this, new OnFiringGunArgs
        {
            mousePosition = mousePosition,
            firePointPosition = firePoint.position
        }) ;

    }
    
    private void Start()
    {

        health = healthMax;
        energy = energyMax;
        rest = restMax;

        canRun = true;
    }



    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    public void HandleRotation()
    {
        if (canRotate)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 playerDirection = (mousePosition - transform.position).normalized;

            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, angle);


        }

    }

    public void HandleMovement()
    {
        

        inputVector = playerMovement.GetMovementNormalize();

        if (IsRunning())
        {
            moveSpeed = 8f ;
            energy -= 20 * Time.deltaTime;
            rest = restMax;
        }
        else
        {
            moveSpeed = 5f;
            
            if (energy < 2 || !canRun)
            {

                moveSpeed = 2f;

                if (rest == 0)
                {
                    canRun = true;
                }
                else
                {
                    canRun = false;
                    RestTimeCountdown();
                }
            }

            if (energy < energyMax)
            {
                energy += 3f * Time.deltaTime;
                
            }
            else
            {
                energy = energyMax;
            }
        }

        if (isEquiped )
        {
            if (isReload)
            {
                moveSpeed = 4f;
            }
            else
            {
                moveSpeed = 2f;
            }
        }

        inputVector *= moveSpeed;

        rb.velocity = inputVector;
    }

    public void RecoverStat()
    {
        health = healthMax;
        energy = energyMax;
    }

    public void ResetStat()
    {
        health *= 1.05f;
        if (health > healthMax)
        {
            health = healthMax; 
        }
    }

    private void RestTimeCountdown()
    {
        if (rest > 0)
        {
            rest -= Time.deltaTime;

        }
        else
        {
            rest = 0;
        }
    }

    public float EnergyPercentage()
    {
         return energy / energyMax;
    }

    public float HealthPercentage()
    {
         return health / healthMax;
    }

    public void AbsorbDamage(float damage)
    {
        health -= damage;
        if (health <= 1f)
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsWalking() 
    {
        if (inputVector != Vector2.zero)
        {
            return true;
        }
        return false;
    }

    public bool IsRunning()
    {
        return PlayerMovement.Instance.isRunning && energy >= 2 && canRun;
    }

   
    


}
