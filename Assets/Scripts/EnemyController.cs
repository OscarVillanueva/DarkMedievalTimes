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

    // Para poder mover al enemigo
    private Rigidbody2D rigidBody;

    // Para poder mover al enemigo;
    private Animator animator;

    // saber si esta muerto
    private bool isDead;

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

        animator = GetComponent<Animator>();
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

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        float distance = Vector3.Distance(transform.position, player.transform.position);
        // TODO: Si la distancia es menor o igual a cierta distancia atacar.

        rigidBody.velocity = new Vector2(direction.normalized.x * speed, direction.normalized.y * speed);
    }

    // Events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // aplicamos daño al jugador si chocamos con el
        if (collision.collider.CompareTag("Player") && !isDead)
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

            animator.SetBool("isDead", true);

            isDead = true;

            Destroy(gameObject, 2.0f);
        }

    }
}
