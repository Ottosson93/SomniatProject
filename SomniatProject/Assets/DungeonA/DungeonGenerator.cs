using Packages.Rider.Editor.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class DungeonGenerator
{
    //int dungeonSize;
    int minRoomSize;

    RNode rootNode;
    public List<RNode> nodes = new List<RNode>();
    List<RNode> finishedNodes = new List<RNode>();

    Material material;



    public DungeonGenerator(Vector2 size, int nbrOfRoom,int roomSize, Material material)
    {
        this.minRoomSize = roomSize;
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x/2, -size.y/2), new Vector2(size.x/2, size.y/2));
        nodes.Add(rootNode);
        this.material = material; 
    }

    public void Generate()
    {

        while (nodes.Count > 0)
        {
            RNode node = nodes[nodes.Count - 1];
            if (node.width > minRoomSize && node.height > minRoomSize)
            {
                Debug.Log("Splitting node of coordinates: " + node.bottomLeft + " and " + node.topRight);
                SplitRoom(node);
            }
            else
            {

                Debug.Log("Bottom node reached");
                node.bottom = true;
                nodes.Remove(node);
                finishedNodes.Add(node);
            }
        }
        /*
        for (int i = 0; i < nodes.Count; i++)
        {
            RNode node = nodes[0];
            //Debug.Log("loop nr: " + i); 
            //Debug.Log("The nr 1 node is: " + node.bottomLeft + " " + node.topRight);

            if (node.width > minRoomSize && node.height > minRoomSize)
            {
                //Debug.Log("Splitting node of size: " + (int)node.width + " and " + (int)node.height);
                SplitRoom(node);
            }
            else
            {
                Debug.Log("Bottom node reached");
                node.bottom = true;
                nodes.Remove(node);
                finishedNodes.Add(node); 
                Debug.Log("Nodes.Count: " + nodes.Count);
                Debug.Log("i: " + i);
            }
            
        }*/

    }

    void SplitRoom(RNode node)
    {
        RNode newNode;
        RNode newNode2;

        //splitting Vertically
        if(node.vertical == false)
        {
            //change name for split sideways to vertical!!!!!!!
            float splitSideways = Random.Range(node.width * 0.25f, node.width * 0.75f);
            Debug.Log("split Vertical = " + splitSideways);
            //float splitV = Random.Range(node.bottomLeft.x * 0.25f, node.bottomLeft.x * 0.75f);

            newNode = new RNode(node.bottomLeft, new Vector2(node.topRight.x - (int)splitSideways, node.topRight.y));
            newNode.parent = node;
            newNode2 = new RNode(new Vector2(node.topRight.x - (int)splitSideways, node.bottomLeft.y), node.topRight);
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
            float splitVertical = Random.Range(node.height * 0.25f, node.height * 0.75f);
            Debug.Log("split sideways = " + splitVertical);
            //float splitH = Random.Range(node.bottomLeft.y, node.topRight.y);

            newNode = new RNode(new Vector2(node.bottomLeft.x, node.topRight.y - (int)splitVertical), node.topRight);
            newNode.parent = node;
            newNode2 = new RNode(node.bottomLeft, new Vector2(node.topRight.x, node.topRight.y - (int)splitVertical));
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
        //int width = Random.Range(node.)
        
    }

    void DeclareFamily() //this method basically connects nodes to their parent + sibling
    {

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
                ShrinkNodes();
                CreateMesh(finishedNodes[i], i);
            }
        }

        
    }

    void ShrinkNodes()
    {
        foreach (RNode n in finishedNodes)
        {
            
            //n.topLeft.y = 0;
            //n.bottomLeft.x = 
        }
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
        GameObject room = new GameObject("floor" + n.bottomLeft.ToString() + ", " + n.topRight.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        room.transform.position = Vector3.zero;
        room.transform.localScale = Vector3.one;
        room.GetComponent<MeshFilter>().mesh = mesh;
        room.GetComponent<MeshRenderer>().material = material;

        //oom.name = "room";
        //room.AddComponent<MeshRenderer>();
    }
}