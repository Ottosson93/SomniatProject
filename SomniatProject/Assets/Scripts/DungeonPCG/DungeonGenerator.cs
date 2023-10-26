﻿using Packages.Rider.Editor.UnitTesting;
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

public class DungeonGenerator : MonoBehaviour
{
    int minRoomSize;

    //Node system variables
    RNode rootNode;
    public List<RNode> nodes = new List<RNode>();
    List<RNode> finishedNodes = new List<RNode>();

    //Room zone variables
    Material greenRoomMaterial;
    Material orangeRoomMaterial;
    Material redRoomMaterial;
    float distFromCenter;
    

    //Room variables
    List<GameObject> preMadeRooms;
    Vector2 largestPreMadeRoom;  //maybe change to two ints: width and height
    List<PreMadeRoom> preMadeRoomNodes = new List<PreMadeRoom>();
    private GameObject horizontalWall, verticalWall, pillar;
    //List<Room> allRooms = new List<Room>();
    
    //Corridor variables
    CorridorGenerator corridorGenerator;
    List<CNode> corridors = new List<CNode>();
    CNode Cnoded;

    List<PCGObjects> objectsToSpawn = new List<PCGObjects>();
    
    int roomID = 1;

    LayerMask Layer;

    Material material;

    //Enemy variables
    List<GameObject> greenEnemyPack = new List<GameObject>();
    List<GameObject> orangeEnemyPack = new List<GameObject>();
    List<GameObject> redEnemyPack = new List<GameObject>();


    public DungeonGenerator(Vector2 size, int nbrOfRoom,int roomSize, Material material, Material greenRoomMaterial, Material orangeRoomMaterial, Material redRoomMaterial, 
        List<GameObject> pmr, GameObject horizontalWall, GameObject verticalWall, GameObject pillar, List<GameObject> greenEnemyPack, List<GameObject> orangeEnemyPack, List<GameObject> redEnemyPack, LayerMask layer)
    {
        this.preMadeRooms = pmr;
        this.verticalWall = verticalWall;
        this.horizontalWall = horizontalWall;
        this.pillar = pillar;
        this.minRoomSize = roomSize;
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x/2, -size.y/2), new Vector2(size.x/2, size.y/2), roomID++);
        rootNode.parent = null;
        rootNode.sibling = null;
        nodes.Add(rootNode);
        this.material = material;
        this.greenRoomMaterial = greenRoomMaterial;
        this.orangeRoomMaterial = orangeRoomMaterial;
        this.redRoomMaterial = redRoomMaterial;
        this.greenEnemyPack = greenEnemyPack;
        this.orangeEnemyPack = orangeEnemyPack;
        this.redEnemyPack = redEnemyPack;
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
            if (finishedNodes[i].bottom == true && finishedNodes[i].manual == false)
            {
                ShrinkNodes(finishedNodes[i]);
                DeclareRoom(finishedNodes[i]);
                CreateRoomMesh(finishedNodes[i], i);
                SpawnEnemy(finishedNodes[i]);
            }
        }
    }

    public void BuildCorridors()
    {
        foreach (RNode r in finishedNodes)
        {
            r.UpdateCorners();
        }
        corridorGenerator = new CorridorGenerator(finishedNodes, pillar);
        corridors = corridorGenerator.GenerateCorridors();
        foreach(CNode c in corridors)
        {
            c.updateWH();
            CreateCorridorMesh(c);
        }

    }
    
    public List<PCGObjects> GetCorridorObjects()
    {
        return corridorGenerator.GetCorridorObjects();
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
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;
        
        //Maybe use an enum instead
        if (n.isGreenRoom == true)
        {
            room.GetComponent<MeshRenderer>().material = greenRoomMaterial;
        }
        else if (n.isOrangeRoom == true)
        {
            room.GetComponent<MeshRenderer>().material = orangeRoomMaterial;
        }
        else if (n.isRedRoom == true)
        {
            room.GetComponent<MeshRenderer>().material = redRoomMaterial;
        }

    }

    private void DeclareRoom(RNode n)
    {
        
        Vector2 center = new Vector2(n.bottomLeft.x + n.width / 2, n.bottomLeft.y + n.height / 2);
        distFromCenter = Vector2.Distance(center, new Vector2(0, 0));
        //Debug.Log(n.id + " " + distFromCenter);
        if (distFromCenter <= 100)
        {
            n.isGreenRoom = true;
        }
        else if (distFromCenter > 100 && distFromCenter <= 150)
        {
            n.isOrangeRoom = true;
        }
        else if (distFromCenter > 150)
        {
            n.isRedRoom = true;
        }
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
        room.GetComponent<MeshRenderer>().material = material;
        room.GetComponent<MeshCollider>().convex = true;
        room.layer = 3;
    }

    public void SpawnEnemy(RNode n)
    {
        Vector3 bottomLeftV = new Vector3(n.bottomLeft.x, 0, n.bottomLeft.y);
        Vector3 center = new Vector3(bottomLeftV.x + n.width / 2, 0, bottomLeftV.z + n.height / 2);

        if (n.isGreenRoom)
        {
            for (int i = 0; i < greenEnemyPack.Count; i++)
            {
                //Change this to change spawn position for enemy
                Vector3 enemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));

                Collider[] intersecting = Physics.OverlapSphere(center + enemyOffset, 2f);

                if (intersecting.Length <= 2)
                {
                    Debug.Log("Theres nothing here (red)");
                    Instantiate(greenEnemyPack[i], center + enemyOffset, Quaternion.identity);
                }
                else if (intersecting.Length > 2)
                {
                    while (intersecting.Length > 2)
                    {
                        Debug.Log("Theres something here! (red) " + (center + enemyOffset));
                        Vector3 newEnemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));
                        Debug.Log("New coordinate " + (center + newEnemyOffset));


                        intersecting = Physics.OverlapSphere(center + newEnemyOffset, 2f);

                        if (intersecting.Length <= 2)
                        {
                            Instantiate(greenEnemyPack[i], center + newEnemyOffset, Quaternion.identity);
                            break;
                        }
                    }
                }
            }
        }
        else if (n.isOrangeRoom)
        {
            for (int i = 0; i < orangeEnemyPack.Count; i++)
            {
                //Change this to change spawn position for enemy
                Vector3 enemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));

                Collider[] intersecting = Physics.OverlapSphere(center + enemyOffset, 2f);

                if (intersecting.Length <= 2)
                {
                    Debug.Log("Theres nothing here (red)");
                    Instantiate(orangeEnemyPack[i], center + enemyOffset, Quaternion.identity);
                }
                else if (intersecting.Length > 2)
                {
                    while (intersecting.Length > 2)
                    {
                        Debug.Log("Theres something here! (red) " + (center + enemyOffset));
                        Vector3 newEnemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));
                        Debug.Log("New coordinate " + (center + newEnemyOffset));


                        intersecting = Physics.OverlapSphere(center + newEnemyOffset, 2f);

                        if (intersecting.Length <= 2)
                        {
                            Instantiate(orangeEnemyPack[i], center + newEnemyOffset, Quaternion.identity);
                            break;
                        }
                    }
                }
            }
        }
        else if (n.isRedRoom)
        {
            for (int i = 0; i < redEnemyPack.Count; i++)
            {
                //Change this to change spawn position for enemy
                Vector3 enemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));

                Collider[] intersecting = Physics.OverlapSphere(center + enemyOffset, 2f);
                Debug.Log("count red " + intersecting.Length + (center + enemyOffset));

                if (intersecting.Length <= 2)
                {
                    Debug.Log("Theres nothing here (red)");
                    Instantiate(redEnemyPack[i], center + enemyOffset, Quaternion.identity);
                }
                else if (intersecting.Length > 2)
                {
                    while (intersecting.Length > 2)
                    {
                        Debug.Log("Theres something here! (red) " + (center + enemyOffset));
                        Vector3 newEnemyOffset = new Vector3(Random.Range(-n.width / 2.5f, n.width / 2.5f), 0, Random.Range(-n.height / 2.5f, n.height / 2.5f));
                        Debug.Log("New coordinate " + (center + newEnemyOffset));


                        intersecting = Physics.OverlapSphere(center + newEnemyOffset, 2f);

                        if (intersecting.Length <= 2)
                        {
                            Instantiate(redEnemyPack[i], center + newEnemyOffset, Quaternion.identity);
                            break;
                        }
                    }
                }



                //if (isObjectHere(enemyOffset))
                //{
                //    Debug.Log("Theres something here");
                //    Instantiate(redEnemyPack[i], center + enemyOffset, Quaternion.identity);
                //}
                //else
                //{
                //    Debug.Log("Theres nothing here");
                //    return;
                //}
            }
        }
        
    }

    //bool isObjectHere(Vector3 currentPosition)
    //{
    //    Collider[] intersecting = Physics.OverlapSphere(currentPosition, 0.01f);
    //    if (intersecting.Length == 0)
    //    {
    //        return false;
    //    }
    //    else if (intersecting.Length != 0)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
