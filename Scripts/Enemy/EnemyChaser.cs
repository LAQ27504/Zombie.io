using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private GameObject zombieVisual;
    [SerializeField] private Rigidbody2D rb;

    private Player player;

    private Enemy enemy;

    private void Awake()
    {
        player = Player.Instance;

        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
   
        Chase();

    }


    private void Chase()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

        zombieVisual.transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        Vector2 inputVector = (player.transform.position - transform.position).normalized;

        inputVector *= Enemy.speed;

        rb.velocity = inputVector;
    }
}
