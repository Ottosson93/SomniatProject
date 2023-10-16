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
using Unity.VisualScripting.Antlr3.Runtime;

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
    //List<Room> allRooms = new List<Room>();
    CorridorGenerator corridorGenerator;
    List<CNode> corridors = new List<CNode>();
    CNode Cnoded;
    int roomID = 1;

    LayerMask Layer;

    Material material;

    //Enemy variables
    //GameObject enemyPrefab = new GameObject();
    List<GameObject> enemyList = new List<GameObject>();

    public DungeonGenerator(Vector2 size, int nbrOfRoom,int roomSize, Material material, List<GameObject> pmr, List<GameObject> enemyList, LayerMask layer)
    {
        this.preMadeRooms = pmr;
        this.minRoomSize = roomSize;
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x/2, -size.y/2), new Vector2(size.x/2, size.y/2), roomID++);
        rootNode.parent = null;
        rootNode.sibling = null;
        nodes.Add(rootNode);
        this.material = material; 
        this.enemyList = enemyList;
        this.Layer = layer;
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

    void ManageSplit(RNode parentNode)
    {
        if(preMadeRooms.Count > 0)
        {
            largestPreMadeRoom.x = preMadeRooms[preMadeRooms.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
            largestPreMadeRoom.y = preMadeRooms[preMadeRooms.Count - 1].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;
            GameObject manualRoom = preMadeRooms[preMadeRooms.Count - 1];

            if ((parentNode.width > (largestPreMadeRoom.x) && parentNode.width < (largestPreMadeRoom.x * 2) && parentNode.height >= largestPreMadeRoom.y)
            || (parentNode.height > (largestPreMadeRoom.y) && parentNode.height < (largestPreMadeRoom.y * 2) && parentNode.width >= largestPreMadeRoom.x))
            {
                float offsetWidth = Random.Range(largestPreMadeRoom.x * 0.25f, largestPreMadeRoom.x * 0.4f);
                float offsetHeight = Random.Range(largestPreMadeRoom.y * 0.25f, largestPreMadeRoom.y * 0.4f);
                PreMadeRoom pmr;
                Vector3 posM;
                RNode newNode;
                Vector2 bl;
                Vector2 tr;
                if (parentNode.width - largestPreMadeRoom.x > parentNode.height - largestPreMadeRoom.y)
                {

                    //this is the node that hold the object and and coordinates of the manual room;
                    posM = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + offsetWidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2);
                    bl = new Vector2(parentNode.bottomLeft.x + offsetWidth, parentNode.bottomLeft.y);
                    tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y + largestPreMadeRoom.y);

                    newNode = new RNode(new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y), parentNode.topRight, roomID++);
                    //Change ^ if adding another node of remaining space

                }
                else
                {
                    posM = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + offsetHeight);
                    bl = new Vector2(parentNode.bottomLeft.x, parentNode.bottomLeft.y + offsetHeight);
                    tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight);

                    newNode = new RNode(new Vector2(parentNode.bottomLeft.x, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight), parentNode.topRight, roomID++);
                    //Change ^ if adding another node of remaining space

                }
                pmr = new PreMadeRoom(posM, manualRoom);
                preMadeRoomNodes.Add(pmr);

                RNode manualRNode = new RNode(bl, tr, roomID++);
                Debug.Log("Manual Room " + manualRNode.id);
                Debug.Log("Manual coordinates: " + bl + tr);
                manualRNode.parent = parentNode;
                manualRNode.sibling = newNode;
                manualRNode.bottom = true;
                manualRNode.maunal = true;


                newNode.parent = parentNode;
                newNode.sibling = manualRNode;
                newNode.vertical = false;
                nodes.Add(newNode);
                parentNode.children.Add(manualRNode);
                parentNode.children.Add(newNode);

                finishedNodes.Add(manualRNode);
                finishedNodes.Add(parentNode);
                nodes.Remove(parentNode);

                preMadeRooms.Remove(manualRoom);
            }
            else
            {
                SplitRoom(parentNode);
            }
        }
        else
        {
            SplitRoom(parentNode);
        }
    }

    void SplitRoom(RNode parentNode)
    {
        RNode newNode;
        RNode newNode2;

        //splitting Vertically
        if (parentNode.vertical == false)
        {
            float splitV = Random.Range(parentNode.width * 0.25f, parentNode.width * 0.75f);

            newNode = new RNode(parentNode.bottomLeft, new Vector2(parentNode.topRight.x - (int)splitV, parentNode.topRight.y), roomID++);
            newNode.parent = parentNode;
            newNode2 = new RNode(new Vector2(parentNode.topRight.x - (int)splitV, parentNode.bottomLeft.y), parentNode.topRight, roomID++);
            newNode2.parent = parentNode;
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
            float splitH = Random.Range(parentNode.height * 0.25f, parentNode.height * 0.75f);

            newNode = new RNode(new Vector2(parentNode.bottomLeft.x, parentNode.topRight.y - (int)splitH), parentNode.topRight, roomID++);
            newNode.parent = parentNode;
            newNode2 = new RNode(parentNode.bottomLeft, new Vector2(parentNode.topRight.x, parentNode.topRight.y - (int)splitH), roomID++);
            newNode2.parent = parentNode;
            newNode.sibling = newNode2;
            newNode2.sibling = newNode;
            newNode.vertical = false;
            newNode2.vertical = false;
            nodes.Add(newNode);
            nodes.Add(newNode2);
        }

        parentNode.children.Add(newNode);
        parentNode.children.Add(newNode2);

        finishedNodes.Add(parentNode);
        nodes.Remove(parentNode);

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
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true && finishedNodes[i].maunal == false)
            {
                ShrinkNodes(finishedNodes[i]);
                CreateMesh(finishedNodes[i], i);
                SpawnEnemy(finishedNodes[i]);
            }
        }
    }

    public void BuildCorridors()
    {
        Debug.Log("--------Corridors-------");
        Debug.Log("finished nodes count: " + finishedNodes.Count);

        foreach (RNode n in finishedNodes)
        {

            if(n.parent != null && n.sibling != null)
            {
                Debug.Log("Node: " + n.id + " sibling " + n.sibling.id + " parent " + n.parent.id);
            }
            else
                Debug.Log("Node: " + n.id + " sibling: root"  + " parent: root");

        }
        foreach (RNode r in finishedNodes)
        {
            r.UpdateCorners();
        }
        corridorGenerator = new CorridorGenerator(finishedNodes);
        corridors = corridorGenerator.GenerateCorridors();
        foreach(CNode c in corridors)
        {
            c.updateWH();
            Debug.Log("C width: " + c.width + " height: " + c.height);
            CreateM(c);
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
        n.UpdateCorners();
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
        GameObject room = new GameObject("floor" + n.id.ToString() + " sibling " + n.sibling.id.ToString() + " parent " + n.parent.id.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider), typeof(MeshCollider));
        //room.transform.position = Vector3.zero;
        //room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        room.GetComponent<BoxCollider>().size = new Vector3(n.width, 0, n.height);
        Vector3 center = new Vector3(bottomLeftV.x + n.width / 2, 0, bottomLeftV.z + n.height / 2);
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;
        
    }
    void CreateM(CNode n)
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
        GameObject room = new GameObject("Corridor: " + n.id, typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider), typeof(MeshCollider));
        //room.transform.position = Vector3.zero;
        //room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        room.GetComponent<BoxCollider>().size = new Vector3(n.width, 0, n.height);
        Vector3 center = new Vector3(bottomLeftV.x + n.width / 2, 0, bottomLeftV.z + n.height / 2);
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;
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
