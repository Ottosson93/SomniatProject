using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private int maxNumberOfRooms;
    DungeonGenerator generator;


    // Start is called before the first frame update
    void Start()
    {
        generator = new DungeonGenerator(size, maxNumberOfRooms);
        generator.Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
