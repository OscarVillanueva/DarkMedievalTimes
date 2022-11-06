using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawnerController : MonoBehaviour
{

    [SerializeField] private GameObject healthPack;
    [SerializeField] private int totalOfPacks = 1;
    [SerializeField] float spawnRadius = 10;
    
    private int healthPackDelay = 10;
    private int totalOfPacksSpawned;

    // Start is called before the first frame update
    void Start()
    {
        healthPackDelay = Random.Range(3, 5);
        StartSpawing();
    }

    public void StartSpawing()
    {
        StartCoroutine(SpawnHealPackRoutine());
    }

    IEnumerator SpawnHealPackRoutine()
    {
        while(totalOfPacksSpawned < totalOfPacks)
        {
            yield return new WaitForSeconds(healthPackDelay);

            // Apartir del spawner tener un radio de 10 para generar 
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;

            Instantiate(healthPack, randomPosition, Quaternion.identity);

            totalOfPacksSpawned = totalOfPacksSpawned + 1;
        }
    }
}
