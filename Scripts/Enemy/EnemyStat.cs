using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{

    public float baseDamage;
    public float baseHealth { get; protected set; }
    public float baseSpeed;
    protected float baseRanged;
    protected float baseAttackCD;

    private void Awake()
    {
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

}
