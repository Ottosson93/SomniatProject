using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class DungeonGenerator
{
    RNode rootNode;
    public List<RNode> nodes = new List<RNode>();
    List<RNode> oldNodes = new List<RNode>();
    
    

    public DungeonGenerator(Vector2 size, int nbrOfRoom)
    {
        //making the rootnode centered with size/2 being the center in both x and y dimensions
        rootNode = new RNode(new Vector2(-size.x/2, -size.y/2), new Vector2(size.x/2, size.y/2));
        nodes.Add(rootNode);
        
    }

    public void Generate()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            RNode node = nodes[0];
            Debug.Log("The nr 1 node is: " + node.bottomLeft + " " + node.topRight);

            if (node.width > 200 && node.height > 200)
                SplitRoom(node);
            else
            {
                node.bottom = true;
                nodes.Remove(node);
                oldNodes.Add(node);
            }
        }

        foreach (RNode node in nodes)
        {
            // 200 minimum allowed size for now
            
        }

        foreach (RNode n in nodes)
            Debug.Log(n.bottomLeft + " " + n.topRight);
    }

    void SplitRoom(RNode node)
    {
        RNode newNode;
        RNode newNode2;

        //horizontal split
        //if vertial split was done last time, do horizontal
        if(node.vertical == false)
        {
            float splitV = Random.Range(node.bottomLeft.x * 0.25f, node.bottomLeft.x * 0.75f);
            newNode = new RNode(node.bottomLeft, new Vector2(splitV, node.topRight.y));
            newNode.parent = node;
            newNode2 = new RNode(new Vector2(splitV, node.bottomLeft.y), node.topRight);
            newNode2.parent = node;
            newNode.sibling = newNode2;
            newNode2.sibling = newNode;
            newNode.vertical = true;
            newNode2.vertical = true;
            nodes.Add(newNode);
            nodes.Add(newNode2);
        } 
        


        if (node.vertical == true)
        {
            float splitH = Random.Range(node.bottomLeft.y, node.topRight.y);
            newNode = new RNode(new Vector2(node.bottomLeft.x, splitH), node.topRight);
            newNode.parent = node;
            newNode2 = new RNode(node.bottomLeft, new Vector2(node.topRight.x, splitH));
            newNode2.parent = node;
            newNode.sibling = newNode2;
            newNode2.sibling = newNode;
            newNode.vertical = false;
            newNode2.vertical = false;
            nodes.Add(newNode);
            nodes.Add(newNode2);
        }

        oldNodes.Add(node);
        nodes.Remove(node);
        Debug.Log("There are " + nodes.Count + " nodes");
        //int width = Random.Range(node.)
    }

    void DeclareFamily() //this method basically connects nodes to their parent + sibling
    {

    }

    public void BuildRooms()
    {
        GameObject room = new GameObject("floor", typeof(MeshFilter), typeof(MeshRenderer));

        
        //oom.name = "room";
        //room.AddComponent<MeshRenderer>();

        
    }
}