using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] GameObject jailDoor1, jailDoor2;
    [SerializeField] Transform startPos1, startPos2;
    [SerializeField] Transform destinationPos1, destinationPos2;
    private bool closing1, closing2;
    private bool opening;
    [SerializeField]List<GameObject> enemies;
    float timer;

    void Start()
    {
        direction = new Vector3(0, 0, 1);
        closing1 = false;
        closing2 = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2)
        {
            int deadEnemies = 0;
            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    
                    deadEnemies++;
                }
            }
            if(deadEnemies == enemies.Count)
            {
                opening = true;
            }
            Debug.Log("dead enemies " + deadEnemies);
            Debug.Log("enemy list " + enemies.Count);
            timer = 0;
        }
        if (closing1)
        {
            Debug.Log("closing1");
            //jailDoor1.transform.position = jailDoor1.transform.position + direction * Time.deltaTime * 5;
            if (Vector3.Distance(jailDoor1.transform.position, destinationPos1.position) > 0.1)
                jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, destinationPos1.position, Time.deltaTime * 5f);
            else
            {
                closing1 = false;
            }
        }
        if (closing2)
        {
            Debug.Log("closing2");
            if (Vector3.Distance(jailDoor2.transform.position, destinationPos2.position) > 0.1)
                jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, destinationPos2.position, Time.deltaTime * 5f);
            else
                closing2 = false;
        }

        if(opening)
        {
            Open();
        }

    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Debug.Log("Closing Doors");
            closing1 = true;
            closing2 = true;
            //Open(other);
        }
    }
    

    private void Close()
    {
        
    }
    private void Open()
    {
        Debug.Log("opening");
        if (Vector3.Distance(jailDoor1.transform.position, startPos1.position) > 0.1)
        {
            Debug.Log("MOVE");
            jailDoor1.transform.position = Vector3.MoveTowards(jailDoor1.transform.position, startPos1.position, Time.deltaTime * 5f);
        }
        //jailDoor2.transform.position = jailDoor2.transform.position - direction * Time.deltaTime * 5;
        if (Vector3.Distance(jailDoor2.transform.position, startPos2.position) > 0.1)
            jailDoor2.transform.position = Vector3.MoveTowards(jailDoor2.transform.position, startPos2.position, Time.deltaTime * 5f);
    }

}
