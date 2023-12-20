using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WallMaskScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Queue<GameObject> gameObjects;

    //private CapsuleCollider collider;
    void Start()
    {
        // collider.GetComponent<CapsuleCollider>();
        //gameObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        gameObjects = new Queue<GameObject>();

        //  for (int i = 0; i < gameObjects.Length; i++)
        //  {
        //      foreach (var item in gameObjects[i].GetComponentsInChildren<MeshRenderer>())
        //          item.material.renderQueue = 3002;
        //  }
    }

    void FixedUpdate()
    {
        // Your fixed-time update code here
    }



    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (!gameObjects.Contains(other.gameObject))
            {
                gameObjects.Enqueue(other.gameObject);
                if (gameObjects.Count >= 8)
                {
                    var oldWall = gameObjects.Dequeue();
                    foreach (var item in oldWall.GetComponentsInChildren<MeshRenderer>())
                    {
                        item.material.renderQueue = -1;
                        Debug.Log($"Popping {item.transform.name}");
                    }
                }

                foreach (var item in other.GetComponentsInChildren<MeshRenderer>())
                {
                    item.material.renderQueue = 3002;
                    Debug.Log($"Sho {other.transform.name}");
                }
            }

        }
    }
}
