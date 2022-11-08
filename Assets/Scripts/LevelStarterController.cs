using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarterController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D exit;
    [SerializeField] private Transform[] enemySpawners;

    private bool isTheLevelStarted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTheLevelStarted)
        {
            GameManager.sharedInstance.StartLevel(exit, enemySpawners);
            isTheLevelStarted = true;
        }
    }
}
