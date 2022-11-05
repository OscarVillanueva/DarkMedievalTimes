using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    // Setter un valor de cuanto va a recuperar este pack de vida
    [SerializeField] private float value = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().RestoreHealth(value);

            Destroy(gameObject, 0.2f);
        }
    }

}
