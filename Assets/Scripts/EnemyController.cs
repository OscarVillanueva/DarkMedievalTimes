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
    public bool isDead;

    // Mover al enemigo en al punto inicial
    public Vector3 initialSpot = Vector3.zero;

    // saber si ya llego al punto inicial
    private bool reachInitialSpot;

    // Saber si el enemigo esta listo para atacar
    private bool isReadyToAttack;

    private void Awake()
    {
        // sacamos el rigid body
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isReadyToAttack = true;

        // inicializamos al player
        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller) player = controller;

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }

        if (reachInitialSpot)
            FollowPlayer();
        else GoToInitialSpot();
    }

    private void GoToInitialSpot()
    {
        float distance = Vector3.Distance(transform.position, initialSpot);

        if (initialSpot == Vector3.zero || distance <= 1)
        {
            reachInitialSpot = true;
            return;
        }

        MoveEnemy(initialSpot);
    }

    private void FollowPlayer()
    {
        // si no hay player lo dejamos de mover
        if (!player)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= 3 && isReadyToAttack)
        {
            animator.SetBool("isAttacking", true);
            StartCoroutine(ReloadWeapon());
        }

        MoveEnemy(player.transform.position);
    }

    private void MoveEnemy(Vector3 goToPosition)
    {
        Vector2 direction = goToPosition - transform.position;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

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

        if (health == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (health <= 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            GameManager.sharedInstance.Score = GameManager.sharedInstance.Score + points;
            EnemyManager.sharedInstance.AnotherEnemyDead();

            animator.SetBool("isDead", true);


            isDead = true;

            Destroy(gameObject, 2.0f);
        }

    }

    IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(1.0f);

        isReadyToAttack = false;
        animator.SetBool("isAttacking", false);

        yield return new WaitForSeconds(2.5f);

        isReadyToAttack = true;
    }
}
