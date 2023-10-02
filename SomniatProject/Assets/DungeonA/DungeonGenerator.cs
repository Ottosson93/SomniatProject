using Packages.Rider.Editor.UnitTesting;
using System.Collections.Generic;
using System.Collections; 
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Rendering.Universal;

public class DungeonGenerator : MonoBehaviour
{
    int minRoomSize;

    //Node system variables
    RNode rootNode;
    public List<RNode> nodes = new List<RNode>();
    List<RNode> finishedNodes = new List<RNode>();
    List<GameObject> preMadeRooms;
    Vector2 largestPreMadeRoom;  //maybe change to two ints: width and height
    List<PreMadeRoom> preMadeRoomNodes = new List<PreMadeRoom>();

    Material material;

    //Enemy variables
    //GameObject enemyPrefab = new GameObject();
    List<GameObject> enemyList = new List<GameObject>();

    public DungeonGenerator(Vector2 size, int nbrOfRoom,int roomSize, Material material, List<GameObject> pmr, List<GameObject> enemyList)
    {
        this.preMadeRooms = pmr;
        this.minRoomSize = roomSize;
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x/2, -size.y/2), new Vector2(size.x/2, size.y/2));
        nodes.Add(rootNode);
        this.material = material; 
        this.enemyList = enemyList;
    }

    public void Generate()
    {
        while (nodes.Count > 0)
        {
            int takeRandom = Random.Range(0, nodes.Count);
            RNode node = nodes[takeRandom];

            //REVERT BACK TO PREVIOUS VERSION WITHOUT MANUAL ROOMS THAT WORKS: JUST CHANGE "ManageSplit()" with "SplitRoom()";
            if (node.width > minRoomSize && node.height > minRoomSize)
            {
                ManageSplit(node);
                //SplitRoom(node);
            }
            
            else if (node.width > minRoomSize * 1.5 && node.height < minRoomSize) //REMOVE?
            {
                
                node.vertical = false;
                ManageSplit(node);
                //SplitRoom(node);
            }
            else if (node.width < minRoomSize && node.height > minRoomSize * 1.5) //REMOVE?
            {
                node.vertical = true;
                ManageSplit(node);
                //SplitRoom(node);
            }
            
            else
            {
                node.bottom = true;
                nodes.Remove(node);
                finishedNodes.Add(node);
            }
        }

    }

    void ManageSplit(RNode node)
    {
        if(preMadeRooms.Count > 0)
        {
            largestPreMadeRoom.x = preMadeRooms[preMadeRooms.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
            largestPreMadeRoom.y = preMadeRooms[preMadeRooms.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;
            GameObject manualRoom = preMadeRooms[preMadeRooms.Count - 1];

            if ((node.width > (largestPreMadeRoom.x) && node.width < (largestPreMadeRoom.x * 2) && node.height >= largestPreMadeRoom.y)
            || (node.height > (largestPreMadeRoom.y) && node.height < (largestPreMadeRoom.y * 2) && node.width >= largestPreMadeRoom.x))
            {
                float offsetWidth = Random.Range(largestPreMadeRoom.x * 0.25f, largestPreMadeRoom.x * 0.4f);
                float offsetHeight = Random.Range(largestPreMadeRoom.y * 0.25f, largestPreMadeRoom.y * 0.4f);
                Debug.Log("There is enough space for a manual room");
                if (node.width - largestPreMadeRoom.x > node.height - largestPreMadeRoom.y)
                {

                    //this is the node that hold the object and and coordinates of the manual room;
                    Vector3 posM = new Vector3(node.bottomLeft.x + largestPreMadeRoom.x / 2 + offsetWidth, 0, node.bottomLeft.y + largestPreMadeRoom.y / 2);
                    PreMadeRoom pmr = new PreMadeRoom(posM, manualRoom);
                    preMadeRoomNodes.Add(pmr);

                    RNode newNode = new RNode(new Vector2(node.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, node.bottomLeft.y), node.topRight);
                    //Change ^ if adding another node of remaining space
                    newNode.parent = node;
                    newNode.sibling = null; //change?
                    newNode.vertical = true;
                    nodes.Add(newNode);
                }
                else
                {

                    Vector3 posM = new Vector3(node.bottomLeft.x + largestPreMadeRoom.x / 2, 0, node.bottomLeft.y + largestPreMadeRoom.y / 2 + offsetHeight);
                    PreMadeRoom pmr = new PreMadeRoom(posM, manualRoom);
                    preMadeRoomNodes.Add(pmr);

                    RNode newNode = new RNode(new Vector2(node.bottomLeft.x, node.bottomLeft.y + largestPreMadeRoom.y + offsetHeight), node.topRight);
                    //Change ^ if adding another node of remaining space
                    newNode.parent = node;
                    newNode.sibling = null; //change?
                    newNode.vertical = false;
                    nodes.Add(newNode);
                }

                //--testing here. REMVOVE THIS LATER?
                nodes.Remove(node);
                finishedNodes.Add(node);
                //--this ^

                preMadeRooms.Remove(manualRoom);
            }
            else
            {
                Debug.Log("NOT enough space for manual room");
                SplitRoom(node);
            }
        }
        else
        {
            Debug.Log("No pre-made rooms left. Continuing as usual");
            SplitRoom(node);
        }
    }

    void SplitWithManualRoom(RNode node)
    {

    }

    void SplitRoom(RNode node)
    {
        RNode newNode;
        RNode newNode2;

        //splitting Vertically
        if (node.vertical == false)
        {
            float splitV = Random.Range(node.width * 0.25f, node.width * 0.75f);

            newNode = new RNode(node.bottomLeft, new Vector2(node.topRight.x - (int)splitV, node.topRight.y));
            newNode.parent = node;
            newNode2 = new RNode(new Vector2(node.topRight.x - (int)splitV, node.bottomLeft.y), node.topRight);
            newNode2.parent = node;
            newNode.sibling = newNode2;
            newNode2.sibling = newNode;
            newNode.vertical = true;
            newNode2.vertical = true;


            nodes.Add(newNode);
            nodes.Add(newNode2);
        } 
        
        //splitting Horizontally 
        else
        {
            float splitH = Random.Range(node.height * 0.25f, node.height * 0.75f);

            newNode = new RNode(new Vector2(node.bottomLeft.x, node.topRight.y - (int)splitH), node.topRight);
            newNode.parent = node;
            newNode2 = new RNode(node.bottomLeft, new Vector2(node.topRight.x, node.topRight.y - (int)splitH));
            newNode2.parent = node;
            newNode.sibling = newNode2;
            newNode2.sibling = newNode;
            newNode.vertical = false;
            newNode2.vertical = false;
            nodes.Add(newNode);
            nodes.Add(newNode2);
        }

        finishedNodes.Add(node);
        nodes.Remove(node);
        
    }

    bool CheckSize(RNode n)
    {
        if (n.width < minRoomSize && n.height < minRoomSize)
        {
            return false;
        }
        return true;
    }

    public void BuildRooms()
    {
        Debug.Log("Building " + finishedNodes.Count + " rooms");
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true)
            {
                ShrinkNodes(finishedNodes[i]);
                CreateMesh(finishedNodes[i], i);
                SpawnEnemy(finishedNodes[i]);
            }
        }
    }

    void ShrinkNodes(RNode n)
    {
        float shrinkWidth = Random.Range(n.width * 0.1f, n.width * 0.2f);
        n.bottomLeft.x += shrinkWidth;
        n.topRight.x -= shrinkWidth;
        float shrinkheight = Random.Range(n.height * 0.1f, n.height * 0.2f);
        n.bottomLeft.y += shrinkheight;
        n.topRight.y -= shrinkheight;

        n.UpdateWH();
    }

    public List<PreMadeRoom> GetManualCoordinates()
    {
        return preMadeRoomNodes;
    }

    void CreateMesh(RNode n, int id)
    {
        Mesh mesh = new Mesh();

        Vector3 bottomLeftV = new Vector3(n.bottomLeft.x, 0, n.bottomLeft.y);
        Vector3 bottomRightV = new Vector3(n.topRight.x, 0, n.bottomLeft.y);
        Vector3 topLeftV = new Vector3(n.bottomLeft.x, 0, n.topRight.y);
        Vector3 topRightV = new Vector3(n.topRight.x, 0, n.topRight.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV, topRightV, bottomLeftV , bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        int[] triangles = new int[]
        {
            0, 1, 2, 2, 1, 3
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;


        //GameObject room = new GameObject("floor" + id.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        GameObject room = new GameObject("floor" + n.bottomLeft.ToString() + ", " + n.topRight.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider), typeof(MeshCollider));
        //room.transform.position = Vector3.zero;
        //room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        room.GetComponent<BoxCollider>().size = new Vector3(n.width, 0, n.height);
        Vector3 center = new Vector3(bottomLeftV.x + n.width / 2, 0, bottomLeftV.z + n.height / 2);
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
    }

    public void SpawnEnemy(RNode node)
    {
        Vector3 bottomLeftV = new Vector3(node.bottomLeft.x, 0, node.bottomLeft.y);
        Vector3 center = new Vector3(bottomLeftV.x + node.width / 2, 0, bottomLeftV.z + node.height / 2);

        for (int i = 0; i < enemyList.Count; i++)
        { 
            //Change this to change spawn position for enemy
            Vector3 offsetForEnemy = new Vector3(Random.Range(-node.width / 2.5f, node.width / 2.5f), 0, Random.Range(-node.height / 2.5f, node.height / 2.5f)); 
           
            Instantiate(enemyList[i], center + offsetForEnemy, Quaternion.identity); 

            //todo: get a trigger to check if something has spawned at chosen position
        }
    }
}