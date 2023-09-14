using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        foreach (RNode node in nodes)
        {
            // 200 minimum allowed size for now
            if (node.width > 200 && node.height > 200)
                SplitRoom(node);
            else
                node.bottom = true;
        }

        foreach (RNode n in nodes)
            Debug.Log(n.bottomLeft + " " + n.topRight);
    }

    void SplitRoom(RNode node)
    {
        //horizontal split
        //if vertial split was done last time, do horizontal
        float split = Random.Range(node.bottomLeft.x * 0.25f, node.bottomLeft.x * 0.75f);
        RNode newNode = new RNode(node.bottomLeft, new Vector2(split, node.topRight.y));
        newNode.parent = node;
        RNode newNode2 = new RNode(new Vector2(split, node.bottomLeft.y), node.topRight);
        newNode2.parent = node;
        newNode.sibling = newNode2;
        newNode2.sibling = newNode; 
        nodes.Add(newNode);
        nodes.Add(newNode2);
        oldNodes.Add(node);
        nodes.Remove(node);
        Debug.Log("There are " + nodes.Count + " nodes");
        
        //int width = Random.Range(node.)
    }
}