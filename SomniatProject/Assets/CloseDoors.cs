using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CloseDoors : MonoBehaviour
{
    [SerializeField] private GameObject jailDoor1, jailDoor2;
    [SerializeField] private Transform startPos1, startPos2, destination1, destination2;
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private List<GameObject> spawnLocation;
    private int enemiesToKill;
    private bool closing = false, opening = false, complete = false;
    private float speed = 8, timer;
    private EnemySpawner enemySpawner;

    private void Start()
    {
        foreach (GameObject location in spawnLocation)
        {
            enemySpawner = location.GetComponent<EnemySpawner>();
        }


        //enemiesToKill = enemyList.Count;
    }
    void Update()
    {
        if (complete == false)
        {
            //Closes the hidden doors
            if (closing)
            {
                jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, destination1.position, speed * Time.deltaTime);
                jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, destination2.position, speed * Time.deltaTime);
                if (Vector3.Distance(jailDoor1.transform.position, destination1.position) < 0.1)
                {
                    closing = false;
                }

                if (!closing)
                {
                    for (int i = 0; i < spawnLocation.Count; i++)
                    {
                         StartCoroutine(enemySpawner.SpawnWave());
                    }
                }
            }
            
            
                
            
            //Opens the doors
            if (opening)
            {
                
                jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, startPos1.position, speed * Time.deltaTime);
                jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, startPos2.position, speed * Time.deltaTime);
                if (Vector3.Distance(jailDoor1.transform.position, startPos1.position) < 0.1)
                {
                    opening = false;
                    complete = true;
                }
            }
            
            //if (timer > 2)
            //{
            //    int deadEnemies = 0;
            //    foreach (GameObject obj in enemies)
            //    {
            //        if (obj == null)
            //            deadEnemies++;
            //    }
                
            //    if (deadEnemies >= enemiesToKill)
            //    {
            //        opening = true;
                    
            //    }
            //    timer = 0;
            //}
            
            //timer += Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            closing = true;
        }
    }
}
