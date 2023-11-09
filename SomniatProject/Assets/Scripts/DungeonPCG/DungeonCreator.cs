using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private int maxNumberOfRooms;
    [SerializeField] private int minimumRoomSize;
    DungeonGenerator generator;
    
    [SerializeField] Material material;
    [SerializeField] Material greenRoomMaterial;
    [SerializeField] Material orangeRoomMaterial;
    [SerializeField] Material redRoomMaterial;

    [SerializeField] private List<GameObject> preMadeRooms; //x = width, y = height, z = type;
    [SerializeField] private GameObject horizontalWall5, horizontalWall1, verticalWall1, verticalWall5, pillar;
    private List<PreMadeRoom> preMadeNodes;

    private List<PCGObjects> objects = new List<PCGObjects>();
    



    [SerializeField] private List<GameObject> greenEnemyPack;
    [SerializeField] private List<GameObject> orangeEnemyPack;
    [SerializeField] private List<GameObject> redEnemyPack;
    RNode node;
    
    [SerializeField] private List<GameObject> props = new List<GameObject>();
    
    [SerializeField] NavMeshSurface navSurface;

    // Start is called before the first frame update
    void Start()
    {
        //this gets the size of the plane
        //Vector3 roomSize = preMadeRooms[1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size; 
        //Debug.Log(roomSize);


        generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material, 
            greenRoomMaterial, orangeRoomMaterial, redRoomMaterial, preMadeRooms, 
            horizontalWall1, horizontalWall5, verticalWall1, verticalWall5, pillar, greenEnemyPack, orangeEnemyPack, redEnemyPack, 3, props);


        generator.Generate();
        generator.BuildRooms();
        generator.BuildCorridors();
        preMadeNodes = generator.GetManualCoordinates();
        foreach(PreMadeRoom p in preMadeNodes)
        {
            //fix Locations
            Instantiate(p.preMadeRoom, p.centerPos, Quaternion.identity);
        }
        Debug.Log("MADE IT");
        objects = generator.GetCorridorObjects();
        foreach(PCGObjects obj in objects)
        {
            Instantiate(obj.objectType, obj.spawnPoint, Quaternion.identity);
        }
        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
