using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    //[SerializeField] private GameObject[] enemyArray;
    public GameObject[] enemyArray;
    public int waveNumber;
    [SerializeField] private int timeBetweenWaves;
    //[SerializeField] private int waveNumber;

    public IEnumerator SpawnWave(GameObject spawnLocation)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            Instantiate(enemyArray[i % enemyArray.Length], spawnLocation.transform.position, Quaternion.identity);
            //Debug.Log("Spawn waves of enemies " + enemyArray[i % enemyArray.Length]);
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyArray[1], enemySpawnLocation.position, enemySpawnLocation.rotation);
        //Debug.Log("Spawning Enemy " + enemyArray[1].name);
    }

}
