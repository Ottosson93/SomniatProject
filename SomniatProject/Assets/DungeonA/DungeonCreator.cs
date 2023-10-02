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
    [SerializeField] private List<GameObject> preMadeRooms; //x = width, y = height, z = type;
    private List<PreMadeRoom> preMadeNodes;

    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField]private List<GameObject> enemyList;
    RNode node;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 roomSize = preMadeRooms[1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size; //this gets the size of the plane
        Debug.Log(roomSize);
        
        generator = new DungeonGenerator(size, maxNumberOfRooms, minimumRoomSize, material, preMadeRooms, enemyList);

        generator.Generate();
        generator.BuildRooms();
        //generator.SpawnEnemy(node, enemyList);
        preMadeNodes = generator.GetManualCoordinates();
        foreach(PreMadeRoom p in preMadeNodes)
        {
            //fix Locations 
            Instantiate(p.preMadeRoom, p.centerPos, Quaternion.identity);
        }
        Debug.Log("MADE IT");
    }

    // Update is called once per frame
    void Update()
    {
    }

}
