using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Saber la velocidad del player
    [SerializeField] private float speed = 6.0f;

    // Cantidad de vida
    [SerializeField] private float health = 3.0f;

    // Saber la frecuencia del parpadeo
    [SerializeField] float blinkRate = 0.01f;

    // Saber cuando tiempo de inmunidad tendra el jugador
    [SerializeField] int invulnerabilityTime = 3;

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

    }

    private void FixedUpdate()
    {
        MovePlayer();
        MoveAim();
    }

    /*
        Función para poder mover al jugador
     */
    private void MovePlayer()
    {
        // Sacamos si se esta moviendo vertical u horizontal 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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
        aim.position = transform.position + (Vector3)facingDirection.normalized;
    }

    /*
     * Función para reducir la vida del jugador
     */
    public void TakeDamage()
    {

        if (isInvulnerable) return;

        health = health - 1;

        if (health <= 0)
        {
            GameManager.sharedInstance.CurrentState = GameStates.gameOver;
        }
        else
        {
            isInvulnerable = true;
            StartCoroutine(MakeVulnerableAgain());
            cameraController.Shake(0.3f);
        }

    }

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
