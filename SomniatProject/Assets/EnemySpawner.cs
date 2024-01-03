using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    [SerializeField] private GameObject[] enemyArray;
    [SerializeField] private int waveNumber;

    public IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            Debug.Log("Spawn waves of enemies");
            yield return new WaitForSeconds(3f);
        }
    }


    public void SpawnEnemy()
    {
        Instantiate(enemyArray[1], enemySpawnLocation.position, enemySpawnLocation.rotation);
        Debug.Log("Spawning Enemy");
    }

}
