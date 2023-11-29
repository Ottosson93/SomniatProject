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
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Net;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using BehaviorTree;


public class DungeonGenerator : MonoBehaviour
{
    int minRoomSize;

    //Node system variables
    RNode rootNode;
    public List<RNode> nodes = new List<RNode>();
    List<RNode> finishedNodes = new List<RNode>();

    //Room variables
    public List<GameObject> preMadeRooms;
    Vector2 largestPreMadeRoom;  //maybe change to two ints: width and height
    List<PreMadeRoom> preMadeRoomNodes = new List<PreMadeRoom>();
    private GameObject wall5, wall1, pillar;
    //List<Room> allRooms = new List<Room>();

    private GameObject startRoom;
    //Corridor variables
    CorridorGenerator corridorGenerator;
    List<CNode> corridors = new List<CNode>();

    List<PCGObjects> objectsToSpawn = new List<PCGObjects>();

    CNode Cnoded;

    int roomID = 1;

    LayerMask Layer;

    Material material, lucidMaterial;


    double distFromCenter;
    int amountOfInteractableProps, amountOfProps, amountOfEnemies;

    //Populate room variables
    List<GameObject> listOfAllEnemies = new List<GameObject>();
    float spawnOffset = 3f, boundOffset = 2.5f;


    Collider[] colliders = new Collider[5];
    List<GameObject> interactableProps = new List<GameObject>();
    List<GameObject> props = new List<GameObject>();
    int[] rotationArray = { 90, 180, 270, 360 };

    public DungeonGenerator(Vector2 size, int nbrOfRoom, int roomSize, Material material, Material lucidMaterial, List<GameObject> pmr, GameObject wall1, GameObject wall5,
        GameObject pillar, List<GameObject> listOfAllEnemies, LayerMask layer, List<GameObject> interactableProps, List<GameObject> props)
    {
        this.preMadeRooms = pmr;
        /*
        foreach ( GameObject room in this.preMadeRooms)
        {
            if (room.name == "Start Room")
            {
                this.startRoom = room;
                preMadeRooms.Remove(room);
                break;
            }
        }*/
        this.wall5 = wall5;
        this.wall1 = wall1;
        this.pillar = pillar;
        this.minRoomSize = roomSize;
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x / 2, -size.y / 2), new Vector2(size.x / 2, size.y / 2), roomID++);
        rootNode.parent = null;
        rootNode.sibling = null;
        nodes.Add(rootNode);
        this.material = material;
        this.lucidMaterial = lucidMaterial;
        this.listOfAllEnemies = listOfAllEnemies;
        this.Layer = layer;
        this.interactableProps = interactableProps;
        this.props = props;
    }

    #region Generate BSP dungeon
    public void Generate()
    {
        while (nodes.Count > 0)
        {
            int takeRandom = Random.Range(0, nodes.Count);
            RNode node = nodes[0];
            //Debug.Log("nr of nodes in list: " + nodes.Count);
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
        if (preMadeRooms.Count > 0)
        {
            largestPreMadeRoom.x = preMadeRooms[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
            largestPreMadeRoom.y = preMadeRooms[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;
            GameObject manualRoom = preMadeRooms[0];

            bool manualInsertionPossible = false;

            float distanceFromOrigo = Vector2.Distance(parentNode.centerPos, Vector2.zero);

            for (int i = 0; i < preMadeRooms.Count; i++)
            {
                manualRoom = preMadeRooms[i];
                largestPreMadeRoom.x = preMadeRooms[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
                largestPreMadeRoom.y = preMadeRooms[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;

                if ((parentNode.width > (largestPreMadeRoom.x) && parentNode.width < (largestPreMadeRoom.x * 2) && parentNode.height >= largestPreMadeRoom.y)
                || (parentNode.height > (largestPreMadeRoom.y) && parentNode.height < (largestPreMadeRoom.y * 2) && parentNode.width >= largestPreMadeRoom.x))
                {
                    if (manualRoom.name == "Start Room" && distanceFromOrigo < 75)
                    {
                        Debug.Log("Start Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom);
                        manualInsertionPossible = true;
                        Debug.DrawLine(new Vector3(parentNode.centerPos.x, 0.2f, parentNode.centerPos.y), Vector3.zero, UnityEngine.Color.white, 500);
                        break;
                    }
                    else if (manualRoom.name == "Boss Room" && distanceFromOrigo > 100)
                    {
                        Debug.Log("Boss Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom);
                        manualInsertionPossible = true;
                        break;
                    }
                    else if (manualRoom.name == "Upgrade Room" && (distanceFromOrigo > 25 && distanceFromOrigo < 150))
                    {
                        Debug.Log("Upgrade Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom);
                        manualInsertionPossible = true;
                        break;
                    }
                    else if (manualRoom.name == "Corridor Room" && (distanceFromOrigo > 25 && distanceFromOrigo < 125))
                    {
                        Debug.Log("Corridor Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom);
                        manualInsertionPossible = true;
                        break;

                    }
                    else if (manualRoom.name == "Battle Room" && (distanceFromOrigo > 75 && distanceFromOrigo < 150))
                    {
                        Debug.Log("Battle Room was inteserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom);
                        manualInsertionPossible = true;
                        break;
                    }
                }
            }

            if (!manualInsertionPossible)
            {
                //Debug.Log("No match for manual insertion");
                SplitRoom(parentNode);
            }

            /*

             if ((parentNode.width > (largestPreMadeRoom.x) && parentNode.width < (largestPreMadeRoom.x * 2) && parentNode.height >= largestPreMadeRoom.y)
             || (parentNode.height > (largestPreMadeRoom.y) && parentNode.height < (largestPreMadeRoom.y * 2) && parentNode.width >= largestPreMadeRoom.x))
             {
                 SplitRoomManually(parentNode, manualRoom);
             }
             else
             {
                 SplitRoom(parentNode);
             }
            */
        }
        else
        {
            SplitRoom(parentNode);
        }
    }

    void SplitRoomManually(RNode parentNode, GameObject manualRoom)
    {
        largestPreMadeRoom.x = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        largestPreMadeRoom.y = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;
        float offsetWidth = Random.Range(largestPreMadeRoom.x * 0.25f, largestPreMadeRoom.x * 0.4f);
        float offsetHeight = Random.Range(largestPreMadeRoom.y * 0.25f, largestPreMadeRoom.y * 0.4f);
        PreMadeRoom pmr;
        Vector3 posM;
        RNode newNode;
        Vector2 bl;
        Vector2 tr;
        if (parentNode.width - largestPreMadeRoom.x > parentNode.height - largestPreMadeRoom.y)
        {
            float btheight = (parentNode.height - largestPreMadeRoom.y) / 2;
            //this is the node that hold the object and and coordinates of the manual room;
            posM = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + offsetWidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + btheight);
            bl = new Vector2(parentNode.bottomLeft.x + offsetWidth, parentNode.bottomLeft.y + btheight);
            tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y + largestPreMadeRoom.y + btheight);

            newNode = new RNode(new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y), parentNode.topRight, roomID++);
            //Change ^ if adding another node of remaining space

        }
        else
        {
            float btwidth = (parentNode.width - largestPreMadeRoom.x) / 2;
            posM = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + btwidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + offsetHeight);
            bl = new Vector2(parentNode.bottomLeft.x + btwidth, parentNode.bottomLeft.y + offsetHeight);
            tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + btwidth, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight);

            newNode = new RNode(new Vector2(parentNode.bottomLeft.x, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight), parentNode.topRight, roomID++);
            //Change ^ if adding another node of remaining space

        }
        pmr = new PreMadeRoom(posM, manualRoom);
        preMadeRoomNodes.Add(pmr);

        RNode manualRNode = new RNode(bl, tr, roomID++);
        manualRNode.parent = parentNode;
        manualRNode.sibling = newNode;
        manualRNode.bottom = true;
        manualRNode.manual = true;


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

    public void PlaceStartingRoomInCenter()
    {
        Vector2 origo = new Vector2(0, 0);
        float startRoomWidth = startRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        float startRoomheight = startRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;

        float lowestDistance = Vector2.Distance(finishedNodes[finishedNodes.Count - 1].centerPos, origo);
        RNode mostCenteredRoom = finishedNodes[finishedNodes.Count - 1];
        foreach (RNode node in finishedNodes)
        {
            if (node.bottom && node.manual == false)
            {
                if (Vector2.Distance(node.centerPos, origo) < lowestDistance)
                {
                    mostCenteredRoom = node;
                    lowestDistance = Vector2.Distance(node.centerPos, origo);
                    Debug.Log("YEEEEEEEES");

                }
            }
        }
        RNode sRoom = new RNode(mostCenteredRoom.bottomLeft, new Vector2(mostCenteredRoom.bottomLeft.x + startRoomWidth, mostCenteredRoom.bottomLeft.y + startRoomheight), 999);
        Vector3 cpmr = new Vector3(sRoom.centerPos.x, 0, sRoom.centerPos.y);
        PreMadeRoom pmr = new PreMadeRoom(cpmr, startRoom);
        preMadeRoomNodes.Add(pmr);

        sRoom.parent = mostCenteredRoom.parent;
        sRoom.sibling = mostCenteredRoom.sibling;
        sRoom.bottom = true;
        sRoom.manual = true;
        sRoom.parent.children.Remove(mostCenteredRoom);
        sRoom.parent.children.Add(sRoom);
        //mostCenteredRoom.sibling.sibling = sRoom;
        sRoom.sibling.sibling = sRoom;
        finishedNodes.Add(sRoom);

        finishedNodes.Remove(mostCenteredRoom);
    }
    #endregion



    //Ska denna bort Arvid?
    bool CheckSize(RNode n)
    {
        if (n.width < minRoomSize && n.height < minRoomSize)
        {
            return false;
        }
        return true;
    }

    //Builds the PCG rooms
    public void BuildRooms()
    {
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true && finishedNodes[i].manual == false)
            {
                ShrinkNodes(finishedNodes[i]);
                DeclareRoomType(finishedNodes[i]);
                CreateRoomMesh(finishedNodes[i], i);
                SpawnProps(finishedNodes[i]);
                SpawnEnemy(finishedNodes[i]);
            }
        }
    }

    #region Corridors
    //Builds connections between rooms
    public void BuildCorridors()
    {
        foreach (RNode r in finishedNodes)
        {
            r.UpdateCorners();
        }
        corridorGenerator = new CorridorGenerator(finishedNodes, wall5, wall1, pillar);
        corridors = corridorGenerator.GenerateCorridors();
        foreach (CNode c in corridors)
        {
            c.updateWH();
            CreateCorridorMesh(c);
        }

    }

    public List<PCGObjects> GetCorridorObjects()
    {
        objectsToSpawn = corridorGenerator.GetCorridorObjects();
        foreach (RNode room in finishedNodes)
        {
            if (room.bottom == true)
            {
                //Place Pillars in corners
                PCGObjects obj = new PCGObjects(room.bottomLeft, pillar, Vector3.zero);
                objectsToSpawn.Add(obj);
                obj = new PCGObjects(room.bottomRight, pillar, Vector3.zero);
                objectsToSpawn.Add(obj);
                obj = new PCGObjects(room.topLeft, pillar, Vector3.zero);
                objectsToSpawn.Add(obj);
                obj = new PCGObjects(room.topRight, pillar, Vector3.zero);
                objectsToSpawn.Add(obj);
                //build walls vertically
                //float wallCenter = 2.5f;

                buildWalls(room);
            }
        }
        return objectsToSpawn;
        //return corridorGenerator.GetCorridorObjects();
    }
    #endregion

    #region Building and placing walls
    void buildWalls(RNode room)
    {
        bool doorwayTop = false;
        Doorway topDoorway;
        bool doorwayBottom = false;
        Doorway bottomDoorway;
        bool doorwayLeft = false;
        Doorway leftDoorway;
        bool doorwayRight = false;
        Doorway rightDoorway;


        foreach (Doorway d in room.doorways)
        {
            if (d.vertical == false && room.topRight.y == d.pillarOne.y)
            {
                topDoorway = d;
                doorwayTop = true;
                PlaceWallsHorizontally(room, d, room.topRight.y, true);

            }
            else if (d.vertical == false && room.bottomLeft.y == d.pillarOne.y)
            {
                doorwayBottom = true;
                bottomDoorway = d;
                PlaceWallsHorizontally(room, d, room.bottomLeft.y, true);
            }
            else if (d.vertical == true && room.bottomLeft.x == d.pillarOne.x)
            {
                leftDoorway = d;
                doorwayLeft = true;
                PlaceWallsVertically(room, d, room.bottomLeft.x, true);
            }
            else if (d.vertical == true && room.topRight.x == d.pillarOne.x)
            {
                rightDoorway = d;
                doorwayRight = true;
                PlaceWallsVertically(room, d, room.topRight.x, true);
            }

        }

        if (!doorwayTop)
        {
            PlaceWallsHorizontally(room, null, room.topRight.y, false);
        }
        if (!doorwayBottom)
        {
            PlaceWallsHorizontally(room, null, room.bottomLeft.y, false);
        }
        if (!doorwayLeft)
        {
            PlaceWallsVertically(room, null, room.bottomLeft.x, false);
        }
        if (!doorwayRight)
        {
            PlaceWallsVertically(room, null, room.topRight.x, false);
        }
    }

    void PlaceWallsHorizontally(RNode room, Doorway d, float y, bool doorway)
    {
        float endPoint;
        if (doorway == false)
        {
            endPoint = room.topRight.x;
        }
        else
        {
            endPoint = d.pillarOne.x;
        }
        float buildPos = room.topLeft.x;

        while (buildPos < room.topRight.x)
        {
            if (buildPos + 4.5 < endPoint)
            {
                if (y == room.topRight.y)
                {
                    AddObjectToSpawn(new Vector2(buildPos + 2.5f, y), wall5, Vector3.zero);
                }
                else
                {
                    AddObjectToSpawn(new Vector2(buildPos + 2.5f, y), wall5, new Vector3(0, 180, 0));
                }
                buildPos += 5;
            }
            else if (buildPos + 0.5 < endPoint)
            {
                if (y == room.topRight.y)
                {
                    AddObjectToSpawn(new Vector2(buildPos + 0.5f, y), wall1, Vector3.zero);
                }
                else
                {
                    AddObjectToSpawn(new Vector2(buildPos + 0.5f, y), wall1, new Vector3(0, 180, 0));
                }

                buildPos += 1;
            }
            else if (doorway == true)
            {
                buildPos += 5; //= topDoorway.pillarTwo.x;
                endPoint = room.topRight.x;
            }
            else if (doorway == false)
            {
                break;
            }
        }
    }

    void PlaceWallsVertically(RNode room, Doorway d, float x, bool doorway)
    {
        float endPoint;
        if (doorway == false)
        {
            endPoint = room.topRight.y;
        }
        else
        {
            endPoint = d.pillarOne.y;
        }
        float buildPos = room.bottomLeft.y;

        while (buildPos < room.topRight.y)
        {
            if (buildPos + 4.5 < endPoint)
            {
                if (x == room.bottomLeft.x)
                {
                    AddObjectToSpawn(new Vector2(x, buildPos + 2.5f), wall5, new Vector3(0, 270, 0));
                }
                else
                {
                    AddObjectToSpawn(new Vector2(x, buildPos + 2.5f), wall5, new Vector3(0, 90, 0));
                }
                buildPos += 5;
            }
            else if (buildPos + 0.5 < endPoint)
            {
                if (x == room.bottomLeft.x)
                {
                    AddObjectToSpawn(new Vector2(x, buildPos + 0.5f), wall1, new Vector3(0, 270, 0));
                }
                else
                {
                    AddObjectToSpawn(new Vector2(x, buildPos + 0.5f), wall1, new Vector3(0, 90, 0));
                }
                buildPos += 1;
            }
            else if (doorway == true)
            {
                buildPos += 5; //= topDoorway.pillarTwo.x;
                endPoint = room.topRight.y;
            }
            else if (doorway == false)
            {
                break;
            }
        }
    }
    #endregion

    void AddObjectToSpawn(Vector2 pos, GameObject type, Vector3 rotation)
    {
        PCGObjects obj = new PCGObjects(pos, type, rotation);
        objectsToSpawn.Add(obj);
    }

    public List<PreMadeRoom> GetManualCoordinates()
    {
        return preMadeRoomNodes;
    }

    #region Create mesh for pcg rooms and corridors
    void CreateRoomMesh(RNode n, int id)
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
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0, 1, 2, 2, 1, 3
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;


        //GameObject room = new GameObject("floor" + id.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        GameObject room = new GameObject("floor" + n.id.ToString() + " sibling " + n.sibling.id.ToString() + " parent " + n.parent.id.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider), typeof(MeshCollider), typeof(Texture));
        //room.transform.position = Vector3.zero;
        //room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        room.GetComponent<BoxCollider>().size = new Vector3(n.width, 0, n.height);
        Vector3 center = new Vector3(bottomLeftV.x + n.width / 2, 0, bottomLeftV.z + n.height / 2);
        room.GetComponent<BoxCollider>().center = center;
        room.GetComponent<MeshRenderer>().material = material;
        //room.GetComponent<MeshRenderer>().material.mainTexture = texFloor;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;

        //if (n.isGreenRoom == true)
        //{
        //    room.GetComponent<MeshRenderer>().material = greenRoomMaterial;
        //}
        //else if (n.isOrangeRoom == true)
        //{
        //    room.GetComponent<MeshRenderer>().material = orangeRoomMaterial;
        //}
        //else if (n.isRedRoom == true)
        //{
        //    room.GetComponent<MeshRenderer>().material = redRoomMaterial;
        //}

        GameObject lucidObject = new GameObject("lucidMesh");

        Mesh lucidMesh = new Mesh();
        lucidMesh.vertices = room.GetComponent<MeshFilter>().mesh.vertices;
        lucidMesh.uv = room.GetComponent<MeshFilter>().mesh.uv;
        lucidMesh.triangles = room.GetComponent<MeshFilter>().mesh.triangles;

        lucidObject.AddComponent<MeshFilter>().mesh = lucidMesh;
        lucidObject.AddComponent<MeshRenderer>().material = lucidMaterial; 

        lucidObject.transform.parent = room.transform;
        lucidObject.layer = 10;

    }

    void CreateCorridorMesh(CNode n)
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
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
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
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;

        GameObject lucidObject = new GameObject("lucidMesh");

        Mesh lucidMesh = new Mesh();
        lucidMesh.vertices = room.GetComponent<MeshFilter>().mesh.vertices;
        lucidMesh.uv = room.GetComponent<MeshFilter>().mesh.uv;
        lucidMesh.triangles = room.GetComponent<MeshFilter>().mesh.triangles;

        lucidObject.AddComponent<MeshFilter>().mesh = lucidMesh;
        lucidObject.AddComponent<MeshRenderer>().material = lucidMaterial;

        lucidObject.transform.parent = room.transform;
        lucidObject.layer = 10;

    }
    #endregion

    private void DeclareRoomType(RNode room)
    {

        //Ändra center till n.center, startroom.center
        distFromCenter = Vector2.Distance(room.centerPos, new Vector2(0, 0));
        //Debug.Log(n.id + " " + distFromCenter);
        if (distFromCenter <= 50)
        {
            room.isGreenRoom = true;
        }
        else if (distFromCenter > 50 && distFromCenter <= 100)
        {
            room.isOrangeRoom = true;
        }
        else if (distFromCenter > 100)
        {
            room.isRedRoom = true;
        }
    }

    private double CalculateRoomSize(RNode room)
    {
        double distFromCorner = Vector2.Distance(room.bottomLeft, room.topRight);
        //Debug.Log("Bottom left: " + n.bottomLeft + " Top right: " + n.topRight + " Distance: " + distFromCorner + " ID: " +  n.id);
        return distFromCorner;
    }

    public void SpawnProps(RNode room)
    {
        //Vector3 center = new Vector3(room.bottomLeft.x + room.width / 2, 0, room.bottomLeft.y + room.height / 2);

        double roomSize = CalculateRoomSize(room);

        //Place a specific amount of props depending on the size of the room
        if (roomSize <= 30)
        {
            amountOfInteractableProps = 2;
            amountOfProps = 1;
        }
        else if (roomSize <= 40)
        {
            amountOfInteractableProps = 3;
            amountOfProps = 2;
        }
        else
        {
            amountOfInteractableProps = 5;
            amountOfProps = 5;
        }

        CreateNoninteractableProps(room, amountOfProps);
        CreateInteractableProps(room, amountOfInteractableProps);
    }

    private void CreateNoninteractableProps(RNode room, int amountOfProps)
    {
        //Spawn a pillar (+3 boxes) with a random rotation
        int rndRotation = Random.Range(0, 3);
        Instantiate(props[0], new Vector3(room.centerPos.x, props[0].transform.position.y, room.centerPos.y), Quaternion.Euler(0, rotationArray[rndRotation], 0));

        for (int i = 1; i < amountOfProps; i++)
        {
            Vector3 objOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

            int rndProp = Random.Range(1, props.Count);

            //Checks if the position is occupied, takes the size of the objects collider and checks if there's anything at the spawn position
            Vector3 propBounds = Vector3.zero;

            //Checks if the object has a collider and if it does it collects the size and assigns it to propBounds
            if (props[rndProp].transform.gameObject.GetComponent<BoxCollider>() != null)
            {
                propBounds = props[rndProp].transform.gameObject.GetComponent<BoxCollider>().size;
                Debug.Log("Got the collider of " + props[rndProp].name + " " + propBounds);
            }

            //If the object doesnt have a collider then we instead get the meshrenderer (+ an offset to give the objects some space inbetween)
            if (propBounds == Vector3.zero)
            {
                propBounds = props[rndProp].transform.gameObject.GetComponentInChildren<MeshRenderer>().bounds.size + new Vector3(boundOffset, boundOffset, boundOffset);
                Debug.Log("Got the mesh of " + props[rndProp].name + " " + propBounds);
            }

            int overlapCount;


            //Creates a sphere collider or box depending on whether the object is larger in the Y-direction
            if (propBounds.y > propBounds.z * 1.5 && propBounds.y > propBounds.x * 1.5)
            {
                overlapCount = Physics.OverlapSphereNonAlloc(room.centerPos + objOffset, propBounds.y, colliders);
                //Debug.Log("Bounding sphere for prop: " + props[rndProp].name + propBounds);
            }
            else
            {
                overlapCount = Physics.OverlapBoxNonAlloc(room.centerPos + objOffset, propBounds, colliders);
                //Debug.Log("Bounding box for prop: " + props[rndProp].name + propBounds);
            }

            //If the position isn't occupied then we place an object here, else we create a new position until we find an empty space
            if (overlapCount <= 2)
            {
                Instantiate(props[rndProp], new Vector3(room.centerPos.x + objOffset.x, props[rndProp].transform.position.y, room.centerPos.y + objOffset.z), Quaternion.identity);
            }
            else if (overlapCount > 2)
            {
                while (overlapCount > 2)
                {
                    Vector3 newObjOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

                    overlapCount = Physics.OverlapSphereNonAlloc(room.centerPos + newObjOffset, 2f, colliders);

                    if (overlapCount <= 2)
                    {
                        Instantiate(props[rndProp], new Vector3(room.centerPos.x + newObjOffset.x, props[rndProp].transform.position.y, room.centerPos.y + newObjOffset.z), Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }

    private void CreateInteractableProps(RNode room, int amountOfInteractableProps)
    {
        //This will spawn interactable objects = to the amountOfProps value per room
        //may want to change it to spawn half of the barrels in one half and rest across the room
        for (int i = 0; i < amountOfInteractableProps; i++)
        {

            Vector3 objOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

            int rndProp = Random.Range(0, interactableProps.Count);

            //Checks if the position is occupied, takes the size of the objects meshrenderer (+X in every dimension to get some more distance) and checks if there's anything at the spawn position
            Vector3 propBounds = interactableProps[rndProp].transform.gameObject.GetComponentInChildren<MeshRenderer>().bounds.size + new Vector3(boundOffset, boundOffset, boundOffset);

            //Collider[] intersecting = Physics.OverlapBox(center + objOffset, propBounds);

            //overlapCount = Physics.OverlapSphereNonAlloc(center + objOffset, 2f, colliders);
            int overlapCount = Physics.OverlapBoxNonAlloc(room.centerPos + objOffset, propBounds, colliders);

            if (propBounds.y < propBounds.z && propBounds.y < propBounds.x)
            {
                overlapCount = Physics.OverlapBoxNonAlloc(room.centerPos + objOffset, propBounds, colliders);
            }
            else if (propBounds.y < propBounds.z || propBounds.y < propBounds.x)
            {
                overlapCount = Physics.OverlapSphereNonAlloc(room.centerPos + objOffset, propBounds.y, colliders);
            }

            //Debug.Log("Bounding box for prop: " + interactableProps[rndProp].name + propBounds);

            //If the position isn't occupied then we place an object here, else we create a new position until we find an empty space
            if (overlapCount <= 2)
            {
                Instantiate(interactableProps[rndProp], new Vector3(room.centerPos.x + objOffset.x, interactableProps[rndProp].transform.position.y, room.centerPos.y + objOffset.z), Quaternion.identity);
            }
            else if (overlapCount > 2)
            {
                while (overlapCount > 2)
                {
                    Vector3 newObjOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

                    overlapCount = Physics.OverlapSphereNonAlloc(room.centerPos + newObjOffset, 2f, colliders);

                    if (overlapCount <= 2)
                    {
                        Instantiate(interactableProps[rndProp], new Vector3(room.centerPos.x + newObjOffset.x, interactableProps[rndProp].transform.position.y, room.centerPos.y + newObjOffset.z), Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }

    public void SpawnEnemy(RNode room)
    {

        //amountofenemies cannot exceed the size of listofallenemies
        if (room.isGreenRoom)
        {
            amountOfEnemies = 2;
            SpawnEnemies(room, amountOfEnemies);
        }
        else if (room.isOrangeRoom)
        {
            amountOfEnemies = 3;
            SpawnEnemies(room, amountOfEnemies);
        }
        else if (room.isRedRoom)
        {
            amountOfEnemies = 5;
            SpawnEnemies(room, amountOfEnemies);
        }
    }

    private void SpawnEnemies(RNode room, int amountOfEnemies)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            Vector3 enemyOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

            Vector3 enemyBounds = listOfAllEnemies[i].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().bounds.size;
            //Debug.Log("Bounding box for enemy: " + listOfAllEnemies[i].name + enemyBounds);

            int overlapCount = Physics.OverlapBoxNonAlloc(room.centerPos + enemyOffset, enemyBounds, colliders);

            if (overlapCount <= 2)
            {
                Instantiate(listOfAllEnemies[i], new Vector3(room.centerPos.x + enemyOffset.x, listOfAllEnemies[i].transform.position.y, room.centerPos.y + enemyOffset.z), Quaternion.identity);
            }
            else if (overlapCount > 2)
            {
                while (overlapCount > 2)
                {
                    Vector3 newEnemyOffset = new Vector3(Random.Range(-room.width / spawnOffset, room.width / spawnOffset), 0, Random.Range(-room.height / spawnOffset, room.height / spawnOffset));

                    overlapCount = Physics.OverlapSphereNonAlloc(room.centerPos + newEnemyOffset, 2f, colliders);

                    if (overlapCount <= 2)
                    {
                        Instantiate(listOfAllEnemies[i], new Vector3(room.centerPos.x + newEnemyOffset.x, listOfAllEnemies[i].transform.position.y, room.centerPos.y + newEnemyOffset.z), Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }
}
