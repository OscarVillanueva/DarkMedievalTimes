using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Indicar la cantidad de power de ataque
    [SerializeField] private int powerAttack = 1;

    // saber a quien esta ligado
    [SerializeField] private PlayerController player;

    // Saber si hizo un attak
    private bool isAttacking = false;

    // Sacar la posición del mouse en pantalla
    private Camera mainCamera;

    // Saber la dirección del player
    private Vector2 facingDirection;

    private void Awake()
    {
        // Sacar la posición del mouse
        mainCamera = FindObjectOfType<Camera>();
    }


    private void FixedUpdate()
    {
        MoveWeapon();

        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
           isAttacking = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
           isAttacking = false;
        }
    }

    /*
     * Función para colocarle una arma al jugador
     */
    private void MoveWeapon()
    {
        if (!mainCamera) return;

        // sacamos la posición que tiene el mouse en la pantalla de juego
        facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;

        // Colocamos el arma
        transform.position = player.transform.position + (Vector3)facingDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") && isAttacking)
        {
            collision.GetComponent<EnemyController>().TakeDamage(powerAttack);
        }

    }
}
