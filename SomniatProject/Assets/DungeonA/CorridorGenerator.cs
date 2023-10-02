using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using BehaviorTree;
using UnityEditor.Networking.PlayerConnection;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public class CorridorGenerator
{
    //List
    List<RNode> rooms = new List<RNode>();
    int leafCounter;
    List<CNode> corridors = new List<CNode>();
    List<RNode> leafs = new List<RNode>();
    List<RNode> currentRooms = new List<RNode>();
    int numberOfCorridors = 0; 

    public CorridorGenerator(List<RNode> rooms)
    {
        this.rooms = rooms;
    }

    public List<CNode> GenerateCorridors()
    {
        Debug.Log("total: " + rooms.Count);
        foreach(RNode node in rooms)
        {
            if (node.bottom == true)
            {
                currentRooms.Add(node);
                leafCounter++;
            }
        }
        Debug.Log("leafs: " + leafCounter);
        Debug.Log("parents: " + (rooms.Count - leafCounter));

        while (currentRooms.Count > 0)
        {
            RNode node;
            node = currentRooms[0];

            //if the siblings children aren't connected, then put this node last and continue with other nodes
            for (int i = 0; i < rooms.Count; i++)
            {
                if (!currentRooms.Contains(node.sibling))
                {
                    currentRooms.Remove(node);
                    currentRooms.Add(node);
                    node = currentRooms[0];
                    
                }
                else
                {
                    
                    break;
                }
            }

            currentRooms.Remove(node);
            if (node.parent != null)
            {
                CreateCorridor(node, node.sibling);
                currentRooms.Remove(node);
                currentRooms.Add(node.parent);
                node.connectedWithSibling = true;
                node.sibling.connectedWithSibling = true;
                currentRooms.Remove(node.sibling);
                numberOfCorridors++;
            }
            else 
            {
                Debug.Log("this was the root node");
            }
        }

        Debug.Log("Number of Corridors created: " + numberOfCorridors);
        return corridors;
    }

    public void CreateCorridor(RNode r1, RNode r2)
    {
        float angle = CheckOrientation(r1, r2);
        CNode c;
        //RNode leftCandidate, rightCandidate;
        List<RNode> leftRoomsChildren;
        List<RNode> rightRoomsChildren;
        if (!r1.bottom)
        {
            leftRoomsChildren = GetAllLeafsFromParent(r1);
        }
        else
        {
            leftRoomsChildren = new List<RNode>();
            leftRoomsChildren.Add(r1);
        }
        if (!r2.bottom)
        {
            rightRoomsChildren = GetAllLeafsFromParent(r2);
        }
        else
        {
            rightRoomsChildren = new List<RNode>();
            rightRoomsChildren.Add(r2);
        }

        if ((angle < 45 && angle > -45))
        {
            //Debug.Log("Right");
            
            ConnectRoomsHorizontally(leftRoomsChildren, rightRoomsChildren);
            c = new CNode(new Vector2(r1.bottomRight.x, r1.bottomRight.y + 10), new Vector2(r2.bottomLeft.x, r2.bottomLeft.y + 10), 10);
        }
        else if ((angle > 45 && angle < 135))
        {
            //Debug.Log("Up");
            ConnectRoomsVertically(leftRoomsChildren, rightRoomsChildren);
            c = new CNode(new Vector2(0,0), new Vector2(0,0), 10);
        }
        else if ((angle > -135 && angle < -45))
        {
            //Debug.Log("Down");
            ConnectRoomsVertically(rightRoomsChildren, leftRoomsChildren);
            c = new CNode(new Vector2(0,0), new Vector2(0,0), 10);
        }
        else
        {
            //Debug.Log("Left");
            
            ConnectRoomsHorizontally(rightRoomsChildren, leftRoomsChildren);
            c = new CNode(new Vector2(0,0), new Vector2(0,0), 10);
        }

        r1.connectedWithSibling = true;
        r2.connectedWithSibling = true;

        
        //corridors.Add(c);

    }
    void ConnectRoomsHorizontally(List<RNode> leftRooms, List<RNode> rightRooms)
    {
        RNode leftCandidate = leftRooms[0];
        RNode rightCandidate = rightRooms[0];

        
        if (leftRooms.Count > 0)
        {
            foreach (RNode node in leftRooms)
            {
                if (node.topRight.x > leftCandidate.topRight.x)
                {
                    leftCandidate = node;
                }
            }
        }

        int minY = (int)leftCandidate.bottomLeft.y;
        int maxY = (int)leftCandidate.topRight.y;

        
        Vector2 rightSideComparisonPoint = new Vector2( rightCandidate.bottomLeft.x , rightCandidate.bottomLeft.y + rightCandidate.height / 2);
        float distance = Vector2.Distance(leftCandidate.topRight, rightSideComparisonPoint);
        
        
        //float distance = Math.Abs(test - test1)
        foreach (RNode node in rightRooms)
        {
            
            rightSideComparisonPoint = new Vector2(node.bottomLeft.x, node.bottomLeft.y + node.height / 2);
            float d = Vector2.Distance(leftCandidate.topRight, node.topLeft);
            
            if(d < distance)
            {
                distance = d;
                rightCandidate = node;
            }
            /*
            if((node.bottomLeft.y > minY + 5 && node.topRight.y < maxY - 5) && node.bottomLeft.x - leftCandidate.bottomRight.y > 20)
            {
                Debug.Log("Right: " + rightCandidate.id.ToString() + rightCandidate + rightCandidate.topLeft + rightCandidate.bottomRight + rightCandidate.topRight);
                rightCandidate = node;
            }*/
        }

        Debug.Log("Connecting: " + leftCandidate.id + " with " + rightCandidate.id);
        CNode c = new CNode(new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2 - 3), 
            new Vector2 (rightCandidate.topLeft.x , rightCandidate.topLeft.y - rightCandidate.height / 2 + 3), 
            10);
        
        
        corridors.Add(c);
    }
    
    void ConnectRoomsVertically(List<RNode> topRooms, List<RNode> bottomRooms)
    {

    }


    float CheckOrientation(RNode r1, RNode r2)
    {
        Vector2 center1 = ((Vector2)r1.bottomLeft + r1.topRight) / 2;
        Vector2 center2 = ((Vector2)r2.bottomLeft + r2.topRight) / 2;

        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

        //Debug.Log("Center1: " + center1);
        //Debug.Log("center2: " + center2);
        //Debug.Log("Angle " + angle);

        //Debug.Log("Node 1: " + r1.id + " Node 2: " + r2.id);
        //||
        return angle;
    }

    List<RNode> GetAllLeafsFromParent(RNode parent)
    {
        List<RNode> leafs = new List<RNode>();

        RNode node;

        Queue<RNode> q = new Queue<RNode>();
        q.Enqueue(parent);

        while (q.Count > 0)
        {
            node = q.Peek();
            q.Dequeue();
            
            RNode r1 = node.children[0];
            if (r1.bottom == true)
            {
                leafs.Add(r1);
            }
            else
            {
                q.Enqueue(r1);
            }
            RNode r2 = node.children[1];
            if (r2.bottom == true)
            {
                leafs.Add(r2);
            }
            else
            {
                q.Enqueue(r2);
            }
        }

        return leafs;
    }
}