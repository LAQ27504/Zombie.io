using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI: MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image background;

    private Enemy enemy;
    private float health;
    private float maxHealth;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.OnShooted += Enemy_OnShooted;
        maxHealth = Enemy.baseHealth;
        health = enemy.health;
        UpdateVisual();
    }

    private void Enemy_OnShooted(object sender, Enemy.OnShootedArgs e)
    {
        health = e.health;
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        healthBar.fillAmount = health/ maxHealth;
    }
}
