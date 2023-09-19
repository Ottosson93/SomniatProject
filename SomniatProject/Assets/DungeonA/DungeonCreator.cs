using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private int maxNumberOfRooms;
    [SerializeField] private int minimumRoomSize;
    DungeonGenerator generator;
    [SerializeField] Material material;


    // Start is called before the first frame update
    void Start()
    {
        generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material);
        generator.Generate();
        generator.BuildRooms(); 
    }

    // Update is called once per frame
    void Update()
    {
    }

}
