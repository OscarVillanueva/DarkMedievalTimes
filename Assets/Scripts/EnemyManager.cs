using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private int totalEnemies = 35;
    [SerializeField] private Transform[] enemySpawners;
    [SerializeField] GameObject enemyPreFab;

    [Range(0.1f, 10f)][SerializeField] private float spawnRate = 1;

    private int enemiesSpawned = 0;
    public int enemiesBeated = 0;

    public static EnemyManager sharedInstance;

    private void Awake()
    {
        if (!sharedInstance) sharedInstance = this;
    }

    private void Start()
    {
        StartLevel();
    }

    // Start is called before the first frame update
    public void StartLevel()
    {
        StartCoroutine(SpawnNewEnemy());
    }

    public void AnotherEnemyDead()
    {
        enemiesBeated = enemiesBeated + 1;

        if (enemiesBeated == totalEnemies)
        {
            GameManager.sharedInstance.LevelCompleted();
        }
    }

    IEnumerator SpawnNewEnemy()
    {
        while (enemiesSpawned < totalEnemies)
        {
            yield return new WaitForSeconds(1 / spawnRate);

            int random = Random.Range(0, enemySpawners.Length);

            // 10% de probabilidad que salga el enemigo fuerte
            Instantiate(enemyPreFab, enemySpawners[random].position, Quaternion.identity);

            enemiesSpawned = enemiesSpawned + 1;
        }

        StopAllCoroutines();
    }
}
