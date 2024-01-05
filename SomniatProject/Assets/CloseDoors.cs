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
    [SerializeField] private List<GameObject> enemies;
    int enemiesToKill;
    bool closing = false;
    bool opening = false;
    private float speed = 8;
    float timer;
    bool complete = false;

    private void Start()
    {
        for (int i = 0; i < spawnLocation.Length; i++)
        {
            enemySpawner = spawnLocation[i].GetComponent<EnemySpawner>();
            //Debug.Log("getting locations " + enemySpawner.name);
        }

        enemiesToKill = enemySpawner.waveNumber * spawnLocation.Length;
        //Debug.Log("Enemies to kill " + enemiesToKill);
    }
    void Update()
    {
        if (complete == false)
        {
            if (closing)
            {
                jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, destination1.position, speed * Time.deltaTime);
                jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, destination2.position, speed * Time.deltaTime);
                if (Vector3.Distance(jailDoor1.transform.position, destination1.position) < 0.1)
                {
                    closing = false;
                }
            }

            if (state == State.Spawning)
            {
                if (!spawning)
                {
                    StartSpawning();
                    spawning = true;
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
                //        //    if (deadEnemies >= enemiesToKill)
                //        //    {
                //        //        opening = true;

                //    }
                //    timer = 0;
                //}
                //timer += Time.deltaTime;


                //int deadEnemies = 0;
                //for (int i = 0; i < spawnLocation.Length; i++)
                //{
                //    foreach (GameObject obj in enemySpawner.enemyArray)
                //    {
                //        Debug.Log("Enemies in array " + enemySpawner.enemyArray.Length);
                //        if (obj == null)
                //            deadEnemies++;
                //    }
                //}


                //if (deadEnemies >= enemiesToKill)
                //{
                //    state = State.Opening;

                //}
                //timer = 0;


                //timerTilDoorOpens -= Time.deltaTime;
                //Debug.Log(timerTilDoorOpens);

                //if (timerTilDoorOpens < 0)
                //{
                //    state = State.Opening;
                //}

                timer += Time.deltaTime;
                Debug.Log(timer);

                if (timer > 10)
                {
                    state = State.Opening;
                }
            }

            //Opens the doors
            if (state == State.Opening)
            {

                jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, startPos1.position, speed * Time.deltaTime);
                jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, startPos2.position, speed * Time.deltaTime);
                if (Vector3.Distance(jailDoor1.transform.position, startPos1.position) < 0.1)
                {
                    opening = false;
                    complete = true;
                }
            }
            if (timer > 2)
            {
                int deadEnemies = 0;
                foreach (GameObject obj in enemies)
                {
                    if (obj == null)
                        deadEnemies++;
                }

                if (deadEnemies >= enemiesToKill)
                {
                    opening = true;

                }
                timer = 0;
            }
            timer += Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            state = State.Closing;
            //Debug.Log("Player has entered battle room");
        }
    }
}
