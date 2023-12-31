#if UNITY_EDITOR
using Palmmedia.ReportGenerator.Core;
#endif
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
    [SerializeField] Material lucidMaterial;

    [SerializeField] private List<GameObject> preMadeRooms; //x = width, y = height, z = type;
    [SerializeField] private GameObject wall5, wall1, pillar;
    [SerializeField] private List<GameObject> walls;
    private List<PreMadeRoom> preMadeNodes;

    private List<PCGObjects> objects = new List<PCGObjects>();

    [SerializeField] private List<GameObject> listOfAllEnemies;
    RNode node;

    [SerializeField] private List<GameObject> interactableProps = new List<GameObject>();
    [SerializeField] private List<GameObject> props = new List<GameObject>();

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


        generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material, lucidMaterial, preMadeRoomsBackUp,
            wall1, wall5, pillar, walls, listOfAllEnemies, 3, interactableProps, props);


        generator.Generate();

        Debug.Log("Generating Rooms");
        while (generator.preMadeRooms.Count > 0)
        {
            preMadeRoomsBackUp.Clear();
            foreach (GameObject obj in preMadeRooms)
            {
                preMadeRoomsBackUp.Add(obj);
            }
            //preMadeRoomsBackUp = preMadeRooms;
            Debug.Log("Rebuilding Dungeon to fit all rooms");

            generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material, lucidMaterial, preMadeRoomsBackUp,
            wall1, wall5, pillar, walls, listOfAllEnemies, 3, interactableProps, props);

            generator.Generate();
        }
        //generator.PlaceStartingRoomInCenter();
        generator.BuildRooms();
        generator.BuildCorridors();

        preMadeNodes = generator.GetManualCoordinates();

        foreach (PreMadeRoom p in preMadeNodes)
        {
            if (p.preMadeRoom.name == "Upgrade Room")
            {
                preMadeNodes.Remove(p);
                preMadeNodes.Add(p);
                break;
            }
        }

        foreach(PreMadeRoom p in preMadeNodes)
        {
            //fix Locations
            int onetofour = Random.Range(0, 4);
            if (p.preMadeRoom.name == "Upgrade Room" || p.preMadeRoom.name == "Corridor Room" || p.preMadeRoom.name == "Start Room")
            {
                onetofour = 0;
            }
            Instantiate(p.preMadeRoom, p.centerPos, Quaternion.Euler(0, 90 * onetofour, 0));
        }
        
        objects = generator.GetCorridorObjects();
        foreach(PCGObjects obj in objects)
        {
            Instantiate(obj.objectType, obj.spawnPoint, obj.angle);
        }
        
        //generator.PopulateDungeonWithProps();
        
        navSurface.BuildNavMesh();

        generator.PopulateDungeon();

        //generator.PopulateDungeonWithEnemies();
    }
}
