using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private int totalEnemies = 35;
    [SerializeField] GameObject enemyPreFab;

    [Range(0.1f, 10f)][SerializeField] private float spawnRate = 1;

    private Transform[] enemySpawners;
    private int enemiesSpawned = 0;
    private int enemiesBeated = 0;

    public static EnemyManager sharedInstance;

    private void Awake()
    {
        if (!sharedInstance) sharedInstance = this;
    }

    // Start is called before the first frame update
    public void StartLevel(Transform[] spawners)
    {

        totalEnemies = totalEnemies * PlayerPrefs.GetInt("diff", 1);
        enemySpawners = spawners;
        enemiesSpawned = 0;
        enemiesBeated = 0;
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

            Transform enemySpawn = enemySpawners[random];
            Transform initialSpot = enemySpawn.GetChild(0);

            enemyPreFab.GetComponent<EnemyController>().initialSpot = initialSpot.position;

            Instantiate(enemyPreFab, enemySpawn.position, Quaternion.identity);

            enemiesSpawned = enemiesSpawned + 1;
        }

        StopAllCoroutines();
    }
}
