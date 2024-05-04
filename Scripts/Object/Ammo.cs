using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IDealDamage, IDetechEnemy
{
    [SerializeField]
    private float ammoSpeed = 50f;
    [SerializeField]
    private Player player;

    private float range = 0.5f;

    private IAbsorbDamage enemy_object;

    private static float damage = 10f;

    private Vector3 ammoDir;
    private Vector3 mousePosition;


    private void Update()
    {
        HitPlayer();

        transform.position += ammoDir * ammoSpeed * Time.deltaTime;

    }

    public void Setup(Vector3 dir, float range)
    {
        this.ammoDir = dir;

        Destroy(gameObject, range / ammoSpeed);
    }

    public void HitPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.zero, range);

        if (hit.collider == null)
        {
            return;
        }
        else
        {
            if (hit.collider.GetComponent<IAbsorbDamage>() != null)
            {
                enemy_object = hit.collider.GetComponent<IAbsorbDamage>();
                DealDamage(enemy_object);
            }

            Destroy(gameObject);
        }

    }

    public void ChangeDamage(float damageInput)
    {
        damage = damageInput;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void DealDamage(IAbsorbDamage enemy)
    {
        enemy.AbsorbDamage(damage);
    }
}
