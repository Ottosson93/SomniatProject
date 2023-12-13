using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scr : MonoBehaviour
{
    Collider[] colliders = new Collider[10];
    Vector3 propBounds;
    int overlapCount;


    void Start()
    {
        propBounds = transform.gameObject.GetComponent<BoxCollider>().size;
    }

    void Update()
    {
        overlapCount = Physics.OverlapBoxNonAlloc(transform.GetComponent<Renderer>().bounds.size, propBounds, colliders);
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log(overlapCount + " Other gameobject " + other.gameObject.name);
    //}

    // Declare and initialize a new List of GameObjects called currentCollisions.
    [SerializeField] List<GameObject> currentCollisions = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {

        // Add the GameObject collided with to the list.
        currentCollisions.Add(other.gameObject);

        // Print the entire list to the console.
        foreach (GameObject gObject in currentCollisions)
        {
            Debug.Log(overlapCount + " Other gameobject " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(other.gameObject);

        // Print the entire list to the console.
        foreach (GameObject gObject in currentCollisions)
        {
            Debug.Log("Exiting " + overlapCount + " Other gameobject " + other.gameObject.name);
        }
    }
}
