using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using BehaviorTree;
using UnityEditor.Networking.PlayerConnection;
using UnityEditor.Rendering;
using Unity.VisualScripting;
using UnityEditor.SearchService;

public class CorridorGenerator
{
    //List
    List<RNode> rooms = new List<RNode>();
    int leafCounter;
    List<CNode> corridors = new List<CNode>();
    List<RNode> leafs = new List<RNode>();
    List<RNode> currentRooms = new List<RNode>();
    int numberOfCorridors = 0;
    GameObject horizontalWall5, horizontallWall1, verticalWall5, verticalWall1, pillar;
    List<PCGObjects> corridorObjects = new List<PCGObjects>();

    int idTracker = 0;

    public CorridorGenerator(List<RNode> rooms, GameObject horiztonallWall5, GameObject horiztonallWall1, GameObject verticalWall5, GameObject verticalWall1, GameObject pillar)
    {
        this.rooms = rooms;
        this.pillar = pillar;
        this.horizontalWall5 = horiztonallWall5;
        this.horizontallWall1 = horiztonallWall1;
        this.verticalWall5 = verticalWall5;
        this.verticalWall1 = verticalWall1;
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
        }
        else if ((angle > 45 && angle < 135))
        {
            //Debug.Log("Up");
            ConnectRoomsVertically(leftRoomsChildren, rightRoomsChildren);
        }
        else if ((angle > -135 && angle < -45))
        {
            //Debug.Log("Down");
            ConnectRoomsVertically(rightRoomsChildren, leftRoomsChildren);
        }
        else
        {
            //Debug.Log("Left");
            
            ConnectRoomsHorizontally(rightRoomsChildren, leftRoomsChildren);
        }

        r1.connectedWithSibling = true;
        r2.connectedWithSibling = true;

        
        //corridors.Add(c);

    }
    void ConnectRoomsHorizontally(List<RNode> leftRooms, List<RNode> rightRooms)
    {
        RNode leftCandidate = leftRooms[0];
        RNode rightCandidate = rightRooms[0];
        RNode leftBackUp = leftRooms[0];

        if (leftRooms.Count > 0)
        {
            foreach (RNode node in leftRooms)
            {
                if (node.topRight.x > leftCandidate.topRight.x)
                {
                    leftBackUp = leftCandidate;
                    leftCandidate = node;
                }
            }
        }
        
        Vector2 rightSideComparisonPoint = new Vector2( rightCandidate.bottomLeft.x , rightCandidate.bottomLeft.y + rightCandidate.height / 2);
        Vector2 leftSideComparisonPoint = new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2);
        
        float distance = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);
        
        //float distance = Math.Abs(test - test1)
        foreach (RNode node in rightRooms)
        {
            
            rightSideComparisonPoint = new Vector2(node.bottomLeft.x, node.bottomLeft.y + node.height / 2);
            float d = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);
            
            if(d < distance)
            {
                distance = d;
                rightCandidate = node;
            }
        }

        /*
        if(leftCandidate.bottomLeft.y - rightCandidate.topRight.y < 5)
        {
            Debug.Log("There was no room for a Corridor, So I changed the LEFT CANDIDATE");
            //use leftbackup as candidate instead
            leftCandidate = leftBackUp;
            rightCandidate = rightRooms[0];
            leftSideComparisonPoint = new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2);
            rightSideComparisonPoint = new Vector2(rightCandidate.bottomLeft.x, rightCandidate.bottomLeft.y + rightCandidate.height / 2);

            distance = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);

            //float distance = Math.Abs(test - test1)
            foreach (RNode node in rightRooms)
            {

                rightSideComparisonPoint = new Vector2(node.bottomLeft.x, node.bottomLeft.y + node.height / 2);
                float d = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);

                if (d < distance)
                {
                    distance = d;
                    rightCandidate = node;
                }
            }
        }
        */

        int offsetY = 0;
        if(leftSideComparisonPoint.y + offsetY <= rightCandidate.bottomLeft.y)
        {
            while(leftSideComparisonPoint.y + offsetY <= rightCandidate.bottomLeft.y)
            {
                offsetY = offsetY + 2;
            }
        }
        else if(leftSideComparisonPoint.y + offsetY + 5 >= rightCandidate.topLeft.y)
        {
            while (leftSideComparisonPoint.y + offsetY + 5 >= rightCandidate.topLeft.y)
            {
                offsetY = offsetY - 2;
            }
        }

        CNode c = new CNode(new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2 + offsetY), 
            new Vector2 (rightCandidate.topLeft.x , leftCandidate.bottomRight.y + leftCandidate.height / 2 + offsetY + 5), 
            10, idTracker++);

        c.vertical = false;
        corridors.Add(c);

        Doorway ldr = new Doorway(c.bottomLeft, new Vector2(c.bottomLeft.x, c.topRight.y), true);
        leftCandidate.doorways.Add(ldr);
        Doorway rdr = new Doorway(new Vector2(c.topRight.x, c.bottomLeft.y), c.topRight, true);
        rightCandidate.doorways.Add(rdr);

        AddCorridorObject(ldr.pillarOne, pillar);
        AddCorridorObject(ldr.pillarTwo, pillar);
        AddCorridorObject(rdr.pillarOne, pillar);
        AddCorridorObject(rdr.pillarTwo, pillar);

        float buildPos = c.bottomLeft.x;
        while (buildPos < c.topRight.x)
        {
            if(buildPos + 4.5 < c.topRight.x)
            {
                AddCorridorObject(new Vector2(buildPos + 2.5f , c.topRight.y), horizontalWall5);
                AddCorridorObject(new Vector2(buildPos + 2.5f, c.bottomLeft.y), horizontalWall5);
                buildPos += 5;
            }
            else if (buildPos + 0.5 < c.topRight.x)
            {
                AddCorridorObject(new Vector2(buildPos + 0.5f, c.topRight.y), horizontallWall1);
                AddCorridorObject(new Vector2(buildPos + 0.5f, c.bottomLeft.y), horizontallWall1);
                buildPos += 1;
            }
            else
            {
                break;
            }
        }
    }
    
    void ConnectRoomsVertically(List<RNode> bottomRooms, List<RNode> topRooms)
    {

        RNode bottomCandidate = bottomRooms[0];
        RNode topCandidate = topRooms[0];


        if (bottomRooms.Count > 0)
        {
            foreach (RNode node in bottomRooms)
            {
                if (node.topRight.y > bottomCandidate.topRight.y)
                {
                    bottomCandidate = node;
                }
            }
        }

        //point a low and center on width of a room above
        Vector2 aboveComparisonPoint = new Vector2(topCandidate.bottomLeft.x + topCandidate.width / 2, topCandidate.bottomLeft.y);
        Vector2 belowComparisonPoint = new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2, bottomCandidate.topRight.y);

        float distance = Vector2.Distance(belowComparisonPoint, aboveComparisonPoint);

        foreach (RNode node in topRooms)
        {

            aboveComparisonPoint = new Vector2(node.bottomLeft.x + node.width / 2, node.bottomLeft.y);
            float d = Vector2.Distance(belowComparisonPoint, aboveComparisonPoint);

            if (d < distance)
            {
                distance = d;
                topCandidate = node;
            }
        }

        int offsetX = 0;

        if (belowComparisonPoint.x + offsetX <= topCandidate.bottomLeft.x)
        {
            while (belowComparisonPoint.x + offsetX <= topCandidate.bottomLeft.x)
            {
                offsetX = offsetX + 2;
            }
        }
        else if (belowComparisonPoint.x + offsetX + 5 >= topCandidate.bottomRight.x)
        {
            while (belowComparisonPoint.x + offsetX + 5 >= topCandidate.bottomRight.x)
            {
                offsetX = offsetX - 2;
            }
        }
        CNode c = new CNode(new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2 + offsetX, bottomCandidate.topRight.y), 
            new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2 + offsetX + 5, topCandidate.bottomLeft.y), 
            10, idTracker++);


       c.vertical = true;
       corridors.Add(c);

       Doorway bdr = new Doorway(c.bottomLeft, new Vector2(c.topRight.x, c.bottomLeft.y), false);
       bottomCandidate.doorways.Add(bdr);
       Doorway tdr = new Doorway(new Vector2(c.bottomLeft.x, c.topRight.y), c.topRight, false);
       topCandidate.doorways.Add(tdr);

       
       AddCorridorObject(bdr.pillarOne, pillar);
       AddCorridorObject(bdr.pillarTwo, pillar);
       AddCorridorObject(tdr.pillarOne, pillar);
       AddCorridorObject(tdr.pillarTwo, pillar);

        float buildPos = c.bottomLeft.y;
        while (buildPos < c.topRight.y)
        {
            if (buildPos + 4.5 < c.topRight.y)
            {
                AddCorridorObject(new Vector2(c.topRight.x, buildPos + 2.5f), verticalWall5);
                AddCorridorObject(new Vector2(c.bottomLeft.x, buildPos + 2.5f), verticalWall5);
                buildPos += 5;
            }
            else if (buildPos + 0.5 < c.topRight.y)
            {
                AddCorridorObject(new Vector2(c.topRight.x, buildPos + 0.5f), verticalWall1);
                AddCorridorObject(new Vector2(c.bottomLeft.x, buildPos + 0.5f), verticalWall1);
                buildPos += 1;
            }
            else
            {
                break;
            }
        }
    }

    void AddCorridorObject(Vector2 pos, GameObject type)
    {
        PCGObjects obj = new PCGObjects(pos, type);
        corridorObjects.Add(obj);
    }

    public List<PCGObjects> GetCorridorObjects()
    {
        return corridorObjects;
    }


    float CheckOrientation(RNode r1, RNode r2)
    {
        Vector2 center1 = ((Vector2)r1.bottomLeft + r1.topRight) / 2;
        Vector2 center2 = ((Vector2)r2.bottomLeft + r2.topRight) / 2;

        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

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