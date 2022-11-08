using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Saber la velocidad del player
    [SerializeField] private float speed = 6.0f;

    // Rango de golpeo del jugador
    [SerializeField] private BoxCollider2D weaponCollider;

    // Cantidad de vida
    [SerializeField] private float health = 3.0f;

    // Saber la frecuencia del parpadeo
    [SerializeField] float blinkRate = 0.01f;

    // Saber cuando tiempo de inmunidad tendra el jugador
    [SerializeField] int invulnerabilityTime = 3;

    // Saber cuando poder de ataque tiene
    [SerializeField] int powerAttack = 1;

    // Colocar la mira
    [SerializeField] private Transform aim;

    // Para mover al jugador
    private Rigidbody2D rigidBody;

    // Para hacer el blik
    private SpriteRenderer spriteRenderer;

    // Saber si es invulnerable
    private bool isInvulnerable = false;

    // Mover la camara
    private CameraController cameraController;

    // Sacar la posición del mouse en pantalla
    private Camera mainCamera;

    // Saber la dirección del player
    private Vector2 facingDirection;

    // Sacar el animator
    private Animator animator;

    // Saber si el player esta attacando;
    private bool isAttacking;

    // Saber si ya puede volver a atacar
    private bool isReadyToAttack = true;

    // Saber cuanto es máximo de vida
    private const float MAX_HEALTH = 3.0f;

    // Saber si ya esta muerto
    private bool isDead = false;
     
    // MARK: - LIFECLYCLE
    private void Awake()
    {
        // Sacamos el rigid body y poder mover al player
        rigidBody = GetComponent<Rigidbody2D>();

        // Sacamos el sprite
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Buscamos la camara
        cameraController = FindObjectOfType<CameraController>();

        // Sacar la posición del mouse
        mainCamera = FindObjectOfType<Camera>();

        // Sacamos el animator
        animator = GetComponent<Animator>();

    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1") && isReadyToAttack && !isDead)
        {
            Attack();
        }

    }
    private void FixedUpdate()
    {
        if (!isDead) MovePlayer();
        //MoveAim();
    }

    // MARK: - METHODS
    private void Attack()
    {

        isReadyToAttack = false;
        animator.SetBool("isAttacking", true);
        weaponCollider.enabled = true;

        Invoke(nameof(ReloadSword), 0.5f);

    }


    /*
        Función para poder mover al jugador
     */
    private void MovePlayer()
    {
        // Sacamos si se esta moviendo vertical u horizontal 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Horizontal", horizontal);


        // Creamos un vector de dirección normalizado para que siempre tenga el mismo tamaño
        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

        // Movemos al personaje
        rigidBody.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    private void MoveAim()
    {
        if (!mainCamera) return;

        // sacamos la posición que tiene el mouse en la pantalla de juego
        facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Colocamos el arma
        aim.position = transform.position + (Vector3)facingDirection.normalized * 1.5f;
    }

    /*
     * Función para reducir la vida del jugador
     */
    public void TakeDamage()
    {

        if (isInvulnerable || isAttacking) return;

        health = health - 1;

        if (health < 0)
        {
            animator.SetBool("isDead", true);
            isDead = true;
            Invoke(nameof(FinishGame), 1.5f);
        }
        else
        {
            isInvulnerable = true;
            StartCoroutine(MakeVulnerableAgain());
            cameraController.Shake(0.3f);
            UIStatsManager.sharedInstance.UpdateLifesLabel((int)health);
        }

    }

    public void FinishGame()
    {
        GameManager.sharedInstance.CurrentState = GameStates.gameOver;
    }

    public void RestoreHealth(float moreHealth)
    {
        health = health + moreHealth;
        if (health > MAX_HEALTH) health = 3;

        UIStatsManager.sharedInstance.UpdateLifesLabel((int)health);
    }

    public void ReloadSword()
    {
        isReadyToAttack = true;
        weaponCollider.enabled = false;
        animator.SetBool("isAttacking", false);
    }

    // MARK: - EVENTS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController controller = collision.GetComponent<EnemyController>();
            if (!controller.isDead) controller.TakeDamage(powerAttack);
        }
    }

    // MARK: - ROUTINES
    /*
     * Hacer que el jugador no pueda recibir daño dentro del tiempo establecido
     */
    IEnumerator MakeVulnerableAgain()
    {

        StartCoroutine(BlinkAfterHit());

        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    /*
     * Hacer que el jugado haga un parpadeo cuando recibe un hit
     */
    IEnumerator BlinkAfterHit()
    {

        int times = 10;

        while (times > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(times * blinkRate);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(times * blinkRate);

            times = times - 1;
        }
    }

}
