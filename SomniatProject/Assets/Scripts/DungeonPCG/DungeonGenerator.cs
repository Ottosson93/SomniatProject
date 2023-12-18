using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;


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
    private List<GameObject> walls;
    int OverWriteCounter = 0;
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


    Collider[] objectCollider = new Collider[10];
    List<GameObject> interactableProps = new List<GameObject>();
    List<GameObject> props = new List<GameObject>();
    int[] rotationArray = { 90, 180, 270, 360 };

    public DungeonGenerator(Vector2 size, int nbrOfRoom, int roomSize, Material material, Material lucidMaterial, List<GameObject> pmr, GameObject wall1, GameObject wall5,
        GameObject pillar, List<GameObject> walls, List<GameObject> listOfAllEnemies, LayerMask layer, List<GameObject> interactableProps, List<GameObject> props)
    {
        this.preMadeRooms = pmr;
        this.wall5 = wall5;
        this.wall1 = wall1;
        this.pillar = pillar;
        this.walls = walls;
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
            if (node.width >= minRoomSize && node.height >= minRoomSize)
            { 
                ManageSplit(node);
            }
            else if (node.width > minRoomSize * 1.2 && node.height < minRoomSize)
            {
                node.vertical = false;
                ManageSplit(node);
            }
            else if (node.width < minRoomSize && node.height > minRoomSize * 1.2)
            {
                node.vertical = true;
                ManageSplit(node);
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
                largestPreMadeRoom.x = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
                largestPreMadeRoom.y = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;

                if ((parentNode.width > (largestPreMadeRoom.x * 1.75) && parentNode.width < (largestPreMadeRoom.x * 2.25) && parentNode.height >= largestPreMadeRoom.y)
                || (parentNode.height > (largestPreMadeRoom.y * 1.75) && parentNode.height < (largestPreMadeRoom.y * 2.25) && parentNode.width >= largestPreMadeRoom.x))
                {
                    if (manualRoom.name == "Start Room" && distanceFromOrigo < 50)
                    {
                        //Debug.Log("Start Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom, true);
                        manualInsertionPossible = true;
                        break;
                    }
                    else if (manualRoom.name == "Boss Room" && distanceFromOrigo > 100)
                    {
                        //Debug.Log("Boss Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom, false);
                        manualInsertionPossible = true;
                        break;
                    }
                    else if (manualRoom.name == "Upgrade Room" && (distanceFromOrigo > 25 && distanceFromOrigo < 75))
                    {
                        //Debug.Log("Upgrade Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom, true);
                        manualInsertionPossible = true;
                        break;
                    }
                    else if (manualRoom.name == "Corridor Room" && (distanceFromOrigo > 25 && distanceFromOrigo < 75))
                    {
                        //Debug.Log("Corridor Room was inserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom, true);
                        manualInsertionPossible = true;
                        break;

                    }
                    else if (manualRoom.name == "Battle Room" && (distanceFromOrigo > 75 && distanceFromOrigo < 125))
                    {
                        //Debug.Log("Battle Room was inteserted with the distance of " + (int)distanceFromOrigo);
                        SplitRoomManually(parentNode, manualRoom, false);
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

    //splitting parent node into a procedural room and a manual room
    void SplitRoomManually(RNode parentNode, GameObject manualRoom, bool placeCentered)
    {
        largestPreMadeRoom.x = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        largestPreMadeRoom.y = manualRoom.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds.size.z;
        float offsetWidth = Random.Range(largestPreMadeRoom.x * 0.25f, largestPreMadeRoom.x * 0.4f);
        float offsetHeight = Random.Range(largestPreMadeRoom.y * 0.25f, largestPreMadeRoom.y * 0.4f);
        PreMadeRoom pmr;
        Vector3 PosManual;
        RNode newNode;
        Vector2 bl;
        Vector2 tr;
        if (parentNode.width - largestPreMadeRoom.x > parentNode.height - largestPreMadeRoom.y)
        {
            //Debug.Log("Splitting " + manualRoom.name + " room vertically");
            //Doing vertical Splitt
            if ((placeCentered && parentNode.centerPos.x < 0) || (!placeCentered && parentNode.centerPos.x > 0))
            {
                float btheight = (parentNode.height - largestPreMadeRoom.y) / 2;
                //this is the node that hold the object and and coordinates of the manual room;
                PosManual = new Vector3(parentNode.topRight.x - largestPreMadeRoom.x / 2 - offsetWidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + btheight);
                bl = new Vector2(parentNode.topRight.x - largestPreMadeRoom.x - offsetWidth, parentNode.bottomLeft.y + btheight);
                tr = new Vector2(parentNode.topRight.x - offsetWidth, parentNode.bottomLeft.y + largestPreMadeRoom.y + btheight);

                //placing pcg room to the left
                newNode = new RNode(parentNode.bottomLeft, new Vector2(parentNode.topRight.x - largestPreMadeRoom.x - offsetWidth, parentNode.topRight.y), roomID++);
                //Change ^ if adding another node of remaining space
            }
            else
            {
                float btheight = (parentNode.height - largestPreMadeRoom.y) / 2;
                //this is the node that hold the object and and coordinates of the manual room;
                PosManual = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + offsetWidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + btheight);
                bl = new Vector2(parentNode.bottomLeft.x + offsetWidth, parentNode.bottomLeft.y + btheight);
                tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y + largestPreMadeRoom.y + btheight);
                
                newNode = new RNode(new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + offsetWidth, parentNode.bottomLeft.y), parentNode.topRight, roomID++);
                //Change ^ if adding another node of remaining space
            }

        }
        else
        {
            //Debug.Log("Splitting " + manualRoom.name + " room horionztally");
            if ((placeCentered && parentNode.centerPos.y < 0) || (!placeCentered && parentNode.centerPos.y > 0))
            {
                float btwidth = (parentNode.width - largestPreMadeRoom.x) / 2;
                PosManual = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + btwidth, 0, parentNode.topRight.y - largestPreMadeRoom.y / 2 - offsetHeight);
                bl = new Vector2(parentNode.bottomLeft.x + btwidth, parentNode.topRight.y - largestPreMadeRoom.y - offsetHeight);
                tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + btwidth, parentNode.topRight.y - offsetHeight);

                newNode = new RNode(parentNode.bottomLeft, new Vector2(parentNode.topRight.x, parentNode.topRight.y - largestPreMadeRoom.y - offsetHeight), roomID++);
                //Change ^ if adding another node of remaining space
            }
            else
            {
                float btwidth = (parentNode.width - largestPreMadeRoom.x) / 2;
                PosManual = new Vector3(parentNode.bottomLeft.x + largestPreMadeRoom.x / 2 + btwidth, 0, parentNode.bottomLeft.y + largestPreMadeRoom.y / 2 + offsetHeight);
                bl = new Vector2(parentNode.bottomLeft.x + btwidth, parentNode.bottomLeft.y + offsetHeight);
                tr = new Vector2(parentNode.bottomLeft.x + largestPreMadeRoom.x + btwidth, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight);

                newNode = new RNode(new Vector2(parentNode.bottomLeft.x, parentNode.bottomLeft.y + largestPreMadeRoom.y + offsetHeight), parentNode.topRight, roomID++);
                //Change ^ if adding another node of remaining space
            }
        }
        pmr = new PreMadeRoom(PosManual, manualRoom);
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
            float splitV = Random.Range(parentNode.width * 0.3f, parentNode.width * 0.7f);

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
            float splitH = Random.Range(parentNode.height * 0.3f, parentNode.height * 0.7f);

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

    //Builds the PCG rooms
    public void BuildRooms()
    {
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true)
            {
                //ShrinkNodes(finishedNodes[i]);
                DeclareRoomType(finishedNodes[i]);
                //CreateRoomMesh(finishedNodes[i], i);
                if (finishedNodes[i].manual == false)
                {
                    ShrinkNodes(finishedNodes[i]);
                    CreateRoomMesh(finishedNodes[i], i);
                }
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
                    AddWall(new Vector2(buildPos + 2.5f, y), room, Vector3.zero);
                }
                else
                {
                    AddWall(new Vector2(buildPos + 2.5f, y), room, new Vector3(0, 180, 0));
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
                buildPos += corridorGenerator.corridorWidth; //= topDoorway.pillarTwo.x;
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
                    AddWall(new Vector2(x, buildPos + 2.5f), room, new Vector3(0, 270, 0));
                }
                else
                {
                    AddWall(new Vector2(x, buildPos + 2.5f), room, new Vector3(0, 90, 0));
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
                buildPos += corridorGenerator.corridorWidth; //= topDoorway.pillarTwo.x;
                endPoint = room.topRight.y;
            }
            else if (doorway == false)
            {
                break;
            }
        }
    }
    void AddWall(Vector2 pos, RNode room, Vector3 rotation)
    {
        OverWriteCounter++;
        GameObject wallObject;
        if (OverWriteCounter >= 5)
        {
            wallObject = walls[walls.Count - 4];
            OverWriteCounter = 0;
        }
        else if (OverWriteCounter == 3)
        {
            
            if (room.isGreenRoom == true)
            {
                wallObject = walls[walls.Count - 3];
            }
            else if (room.isOrangeRoom == true)
            {
                wallObject = walls[walls.Count - 2];
            }
            else
            {
                wallObject = walls[walls.Count - 1];
            }
        }
        else
        {
            int i = Random.Range(0, walls.Count - 5);
            wallObject = walls[i];
        }
        
        AddObjectToSpawn(pos, wallObject, rotation);

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
        GameObject room = new GameObject("floor" + n.id.ToString() + " sibling " + n.sibling.id.ToString() + " parent " + n.parent.id.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider), typeof(MeshCollider));
        //room.transform.position = Vector3.zero;
        //room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();
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
        lucidMesh.RecalculateNormals();
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
        mesh.RecalculateNormals();
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
        lucidMesh.RecalculateNormals();
    }
    #endregion


    #region Prop placement

    //public void PopulateDungeon()
    //{
    //    for (int i = 0; i < finishedNodes.Count; i++)
    //    {
    //        if (finishedNodes[i].bottom == true && finishedNodes[i].manual == false)
    //        {
    //            SpawnProps(finishedNodes[i]);
    //            SpawnEnemy(finishedNodes[i]);
    //        }
    //    }
    //}


    public void PopulateDungeonWithEnemies()
    {
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true && finishedNodes[i].manual == false)
            {
                SpawnProps(finishedNodes[i]);
            }
        }
    }

    public void PopulateDungeonWithProps()
    {
        for (int i = 0; i < finishedNodes.Count; i++)
        {
            if (finishedNodes[i].bottom == true && finishedNodes[i].manual == false)
            {
                SpawnProps(finishedNodes[i]);
            }
        }
    }


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

    private float CalculateRoomArea(RNode room)
    {
        float calcArea = room.width * room.height;
        //Debug.Log("Area " + calcArea + "ID " + room.id);
        return calcArea;
    }

    public void SpawnProps(RNode room)
    {
        //Vector3 center = new Vector3(room.bottomLeft.x + room.width / 2, 0, room.bottomLeft.y + room.height / 2);

        float roomSize = CalculateRoomArea(room);

        //Place a specific amount of props (cannot exceed corresponding list size) depending on the size of the room
        if (roomSize <= 300)
        {
            amountOfInteractableProps = 2;
            amountOfProps = 0;
        }
        else if (roomSize > 300 && roomSize < 450)
        {
            amountOfInteractableProps = 2;
            amountOfProps = 1;
        }
        else if (roomSize >= 450 && roomSize < 800)
        {
            amountOfInteractableProps = 3;
            amountOfProps = 2;
        }
        else if (roomSize >= 800 && roomSize < 1100)
        {
            amountOfInteractableProps = 4;
            amountOfProps = 3;
        }
        else if (roomSize >= 1100)
        {
            amountOfInteractableProps = 5;
            amountOfProps = 5;
        }

        CreateNoninteractableProps(room, amountOfProps, roomSize);
        CreateInteractableProps(room, amountOfInteractableProps);
    }
    public void SpawnEnemy(RNode room)
    {
        //amountofenemies cannot exceed the size of listofallenemies
        if (room.isGreenRoom)
        {
            amountOfEnemies = 2;
        }
        else if (room.isOrangeRoom)
        {
            amountOfEnemies = 3;
        }
        else if (room.isRedRoom)
        {
            amountOfEnemies = 4;
        }
        //CreateEnemies(room, amountOfEnemies);
    }

    private void CreateNoninteractableProps(RNode room, int amountOfProps, double roomSize)
    {
        int rndRotation = Random.Range(0, 3);

        //Create a pillar in the middle of the room if the roomsize exceeds the condition
        if (roomSize > 500)
        {
            if (room.isGreenRoom == true)
            {
                Instantiate(props[0], new Vector3(room.centerPos.x, props[0].transform.position.y, room.centerPos.y), Quaternion.Euler(0, rotationArray[rndRotation], 0));
            }
            else if (room.isOrangeRoom == true)
            {
                Instantiate(props[1], new Vector3(room.centerPos.x, props[1].transform.position.y, room.centerPos.y), Quaternion.Euler(0, rotationArray[rndRotation], 0));
            }
            else if (room.isRedRoom == true)
            {
                Instantiate(props[2], new Vector3(room.centerPos.x, props[2].transform.position.y, room.centerPos.y), Quaternion.Euler(0, rotationArray[rndRotation], 0));
            }
            else
                Instantiate(props[0], new Vector3(room.centerPos.x, props[0].transform.position.y, room.centerPos.y), Quaternion.Euler(0, rotationArray[rndRotation], 0));
        }

        for (int i = 0; i < amountOfProps; i++)
        {
            int rndProp = Random.Range(3, props.Count);

            Vector3 propBounds = props[rndProp].transform.gameObject.GetComponentInChildren<BoxCollider>().size;
            Debug.Log("Bounding Size for (box collider) prop: " + props[rndProp].name + propBounds);

            Spawner(props[rndProp], room, propBounds);

            //Code to spawn objects if their siblings are a specific manual room
            //if (room.sibling.manual)
            //{
            //    if (room.sibling.manualRoom.name == "Boss Room")
            //    {
            //        Spawner(props[7], room, propBounds);
            //    }
            //    else if (room.sibling.manualRoom.name == "Shop Room")
            //    {
            //        Spawner(props[8], room, propBounds);
            //    }
            //}
        }
    }

    private void CreateInteractableProps(RNode room, int amountOfInteractableProps)
    {
        //This will spawn interactable objects = to the amountOfProps value per room
        //may want to change it to spawn half of the barrels in one half and rest across the room
        for (int i = 0; i < amountOfInteractableProps; i++)
        {
            int rndProp = Random.Range(0, interactableProps.Count);

            //Takes the size of the objects meshrenderer (+X in every dimension to get some more distance) 
            Vector3 propBounds = interactableProps[rndProp].transform.gameObject.GetComponentInChildren<BoxCollider>().size;
            Debug.Log("Bounding Size (box collider) for prop: " + interactableProps[rndProp].name + propBounds);


            //If the position isn't occupied then we place an object here, else we create a new position until we find an empty space
            Spawner(interactableProps[rndProp], room, propBounds);
        }
    }
    
    private void CreateEnemies(RNode room, int amountOfEnemies)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            Vector3 enemyBounds = listOfAllEnemies[i].transform.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size;
            Debug.Log("Bounding size (skinnedmesh) for enemy: " + listOfAllEnemies[i].name + enemyBounds);

            //Use instead of i (listOfAllEnemies[i]) if you want random enemies to spawn in rooms
            //int rndEnemy = Random.Range(0, listOfAllEnemies.Count);

            Spawner(listOfAllEnemies[i], room, enemyBounds);
        }
    }

    private void Spawner(GameObject spawnObject, RNode room, Vector3 objBoundSize)
    {
        float spawnPositionOffset = 2.5f;

        //Could need a distance check here or in if statement below so objects dont spawn too close to center  + new Vector3(boundOffset, 0, boundOffset)
        Vector3 objOffset = new Vector3(Random.Range(-room.width / spawnPositionOffset, room.width / spawnPositionOffset), 0, Random.Range(-room.height / spawnPositionOffset, room.height / spawnPositionOffset));
        Vector3 spawnPosition = new Vector3(room.centerPos.x + objOffset.x, spawnObject.transform.position.y, room.centerPos.y + objOffset.z);
        //Vector3 boundSize = new Vector3(objBounds.size.x + boundOffset, 0, objBounds.size.z + boundOffset);
        Vector3 roomCenterPos = new Vector3(room.centerPos.x, 0, room.centerPos.y);

        Vector3 objCenterPos = roomCenterPos + objOffset;

        int overlapCount = Physics.OverlapBoxNonAlloc(objCenterPos, objBoundSize / 2, objectCollider);
        
        //Debug.Log("object name " + spawnObject.name + " room id " + room.id + " objects position " + objCenterPos + " room centerpos " + room.centerPos + " objOffset " + objOffset + " objboundscenter " + objBounds.center);

        if (overlapCount <= 2)
        {
            Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            //Debug.Log("overlapCount: " + overlapCount + " There's nothing here " + room.id + " " + spawnObject.name + " noninterprop pos: " + new Vector3(room.centerPos.x + objOffset.x, spawnObject.transform.position.y, room.centerPos.y + objOffset.z));
        }
        else if (overlapCount > 2)
        {
            //Debug.Log("overlapCount BEFORE new gets created: " + overlapCount + " There's something here " + room.id + " " + spawnObject.name + " noninterprop pos: " + new Vector3(room.centerPos.x + objOffset.x, spawnObject.transform.position.y, room.centerPos.y + objOffset.z));

            for (int i = 0; i < 100; i++)
            {
                if (overlapCount > 2)
                {
                    Vector3 newObjOffset = new Vector3(Random.Range(-room.width / spawnPositionOffset, room.width / spawnPositionOffset), 0, Random.Range(-room.height / spawnPositionOffset, room.height / spawnPositionOffset));
                    Vector3 newObjCenterPos = roomCenterPos + newObjOffset;
                    overlapCount = Physics.OverlapBoxNonAlloc(newObjCenterPos, objBoundSize / 2, objectCollider);

                    //Debug.Log("overlapCount AFTER new gets created: " + overlapCount + " " + room.id + " " + spawnObject.name + " noninterprop pos: " + new Vector3(room.centerPos.x + newObjOffset.x, spawnObject.transform.position.y, room.centerPos.y + newObjOffset.z));

                    if (overlapCount <= 2)
                    {
                        Instantiate(spawnObject, new Vector3(room.centerPos.x + newObjOffset.x, spawnObject.transform.position.y, room.centerPos.y + newObjOffset.z), Quaternion.identity);

                        //Debug.Log("overlapCount: " + overlapCount + " There's nothing here " + room.id + " " + spawnObject.name + " noninterprop pos: " + new Vector3(room.centerPos.x + newObjOffset.x, spawnObject.transform.position.y, room.centerPos.y + newObjOffset.z));

                        break;
                    }
                }

                //if (i > 98)
                //{
                //    Debug.Log("i = " + i + " " + room.id + " " + spawnObject.name);
                //}
            }
        }
    }
    #endregion
}
