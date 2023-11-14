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
    [SerializeField] private GameObject wall5, wall1, pillar;
    private List<PreMadeRoom> preMadeNodes;

    private List<PCGObjects> objects = new List<PCGObjects>();




    [SerializeField] private List<GameObject> greenEnemyPack;
    [SerializeField] private List<GameObject> orangeEnemyPack;
    [SerializeField] private List<GameObject> redEnemyPack;
    RNode node;
    
    [SerializeField] NavMeshSurface navSurface;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> preMadeRoomsBackUp = new List<GameObject>();
        foreach(GameObject obj in preMadeRooms)
        {
            preMadeRoomsBackUp.Add(obj);
        }


        //this gets the size of the plane
        //Vector3 roomSize = preMadeRooms[1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size; 
        //Debug.Log(roomSize);


        generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material, 
            greenRoomMaterial, orangeRoomMaterial, redRoomMaterial, preMadeRoomsBackUp, 
            wall1, wall5, pillar, greenEnemyPack, orangeEnemyPack, redEnemyPack, 3);

        Debug.Log("Rooms to place: " + generator.preMadeRooms.Count);

        generator.Generate();

        Debug.Log(generator.preMadeRooms.Count + " Rooms did not get placed");
        while (generator.preMadeRooms.Count > 0)
        {
            preMadeRoomsBackUp.Clear();
            foreach (GameObject obj in preMadeRooms)
            {
                preMadeRoomsBackUp.Add(obj);
            }
            //preMadeRoomsBackUp = preMadeRooms;
            Debug.Log("PreMadeRoomsCount: " + preMadeRooms.Count + " PreMadeRoomsBackUpCount: " + preMadeRoomsBackUp.Count);
            Debug.Log("Rebuilding Dungeon to fit all rooms");
            generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material,
            greenRoomMaterial, orangeRoomMaterial, redRoomMaterial, preMadeRoomsBackUp,
            wall1, wall5, pillar, greenEnemyPack, orangeEnemyPack, redEnemyPack, 3);
            Debug.Log("PreMadeRoomsCount: " + preMadeRooms.Count + " PreMadeRoomsBackUpCount: " + preMadeRoomsBackUp.Count);
            generator.Generate();
        }
        //generator.PlaceStartingRoomInCenter();
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
            Instantiate(obj.objectType, obj.spawnPoint, obj.angle);
        }
        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
