using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    //[SerializeField] private GameObject[] enemyArray;
    public List<GameObject> enemyList, enemiesToKill;
    public int waveNumber;
    [SerializeField] private int timeBetweenWaves;
    //[SerializeField] private int waveNumber;

    public IEnumerator SpawnWave(GameObject spawnLocation)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            Instantiate(enemyList[i % enemyList.Count], spawnLocation.transform.position, Quaternion.identity);
            enemiesToKill.Add(enemyList[i % enemyList.Count]);
            Debug.Log("Spawn waves of enemies " + enemyList[i % enemyList.Count] + " enemies to kill " + enemiesToKill.Count);
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyList[1], enemySpawnLocation.position, enemySpawnLocation.rotation);
        Debug.Log("Spawning Enemy " + enemyList[1].name);
    }

}
