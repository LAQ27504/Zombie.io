using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IAbsorbDamage, IDealDamage, IDetechEnemy
{
    public static Enemy Instance;

    public static event EventHandler<OnKilledArgs> OnKilled;

    public class OnKilledArgs : EventArgs 
    {
        public int score;
        public int bone; 
    }


    public event EventHandler<OnShootedArgs> OnShooted;

   public class OnShootedArgs : EventArgs
   {
        public float health;
   }

    [SerializeField] private GameObject zombieVisual;
    [SerializeField] private FieldOfView fieldOfView;
    public static float speed { get; private set; }

    private static float baseDamage;
    public static float baseHealth { get; protected set; }
    private static float baseSpeed;
    private static float baseRanged;
    private static float baseAttackCD;

    public float health { get; private set; }

    private float damage;

    private Rigidbody2D rb;

    public Player player { get; private set; }

    private float attackCooldown;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayState.Instance.OnDoneSpawnEnemy += Instance_OnDoneSpawnEnemy;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Instance_OnDoneSpawnEnemy(object sender, EventArgs e)
    {
        attackCooldown = baseAttackCD;
        health = baseHealth;
        damage = baseDamage;
        speed = baseSpeed;

        fieldOfView.hitbox = baseRanged;
    }

    private void Update()
    {
        fieldOfView.SetAimDirection(zombieVisual.transform.up);
        fieldOfView.SetOrigin(transform.position);
        HitPlayer();
    }

    private void HitCooldown()
    {
        if (!IsDoneCooldownAttack())
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void HitPlayer()
    {
        if (fieldOfView.AttackPlayer() != null && IsDoneCooldownAttack())
        {
            attackCooldown = baseAttackCD;
            player = fieldOfView.AttackPlayer();
            DealDamage(player);
        }
        else if (!IsDoneCooldownAttack())
        {
            HitCooldown();
        }
    }

    public void PauseEnemy()
    {
        speed = 0f;
    }

    public void UnPauseEnemy()
    {
        speed = baseSpeed;
    }

    public void SetStat(float health, float damage, float speed, float range, float attackCD)
    {
        baseHealth = health;
        baseDamage = damage;
        baseSpeed = speed;
        baseRanged = range;
        baseAttackCD = attackCD;
    }

    public void AddStat(float healthPercentage, float damagePercentage, float speedPercentage, float rangePercentage, float attackCDPercentage)
    {
        baseHealth *= healthPercentage;
        baseDamage *= damagePercentage;
        baseSpeed *= speedPercentage;
        baseRanged *= rangePercentage;
        baseAttackCD *= attackCDPercentage;
    }

    public void AbsorbDamage(float damage)
    { 
        health -= damage;

        OnShooted?.Invoke(this, new OnShootedArgs
        {
            health = health
        });

        if (health <= 0)
        {

            OnKilled?.Invoke(this, new OnKilledArgs
            {
                score = 10,
                bone = 1
            }) ;
            Destroy(gameObject);
        }
    }

    

    public void DealDamage(IAbsorbDamage player) 
    {
        player.AbsorbDamage(damage);
    }

    
    private bool IsDoneCooldownAttack()
    {
        if (attackCooldown <= 0)
        {
            return true;
        }
        return false;
    }

}
