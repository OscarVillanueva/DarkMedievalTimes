using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private int health = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private int points = 100;

    // tener una referencia del player para hacerle daño
    private PlayerController player;

    // PAra poder mover al enemigo
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        // sacamos el rigid body
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // inicializamos al player
        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller) player = controller;
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // si no hay player lo dejamos de mover
        if (!player)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = player.transform.position - transform.position;
        rigidBody.velocity = new Vector2(direction.normalized.x * speed, direction.normalized.y * speed);
    }

    // Events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // aplicamos daño al jugador si chocamos con el
        if (collision.collider.CompareTag("Player"))
        {
            player.TakeDamage();
        }
    }

    // función para recibir daño
    // int damage -> es el daño que queremos que el enemy reciba
    public void TakeDamage(int damage)
    {

        health = health - damage;

        if (health <= 0)
        {
            GameManager.sharedInstance.Score = GameManager.sharedInstance.Score + points;
            EnemyManager.sharedInstance.AnotherEnemyDead();
            Destroy(gameObject, 0.1f);
        }

    }
}
