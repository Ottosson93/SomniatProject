using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private int waveNumber;

    public IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.75f);
        }
    }


    public void SpawnEnemy()
    {
        Instantiate(enemyList[waveNumber], enemySpawnLocation.position, enemySpawnLocation.rotation);
    }

}
