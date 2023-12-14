using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using BehaviorTree;
using Unity.VisualScripting;
using System.Linq;

public class CorridorGenerator
{
    //List
    List<RNode> rooms = new List<RNode>();
    int leafCounter;
    List<CNode> corridors = new List<CNode>();
    List<RNode> leafs = new List<RNode>();
    List<RNode> currentRooms = new List<RNode>();
    int numberOfCorridors = 0;
    public int corridorWidth = 7;
    GameObject Wall5, Wall1, pillar;
    List<PCGObjects> corridorObjects = new List<PCGObjects>();

    List<RNode> recentlyConnectedRnodes = new List<RNode>();

    int idTracker = 0;

    public CorridorGenerator(List<RNode> rooms, GameObject Wall5, GameObject Wall1, GameObject pillar)
    {
        this.rooms = rooms;
        this.pillar = pillar;
        this.Wall5 = Wall5;
        this.Wall1 = Wall1;
    }

    public List<CNode> GenerateCorridors()
    {
        foreach(RNode node in rooms)
        {
            if (node.bottom == true)
            {
                currentRooms.Add(node);
                leafCounter++;
            }
        }

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
        }
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

        //if you want to connect more often and not type manually:
        //float candidates = rightRoomsChildren.Count + leftRoomsChildren.Count;
        //int connections = (int)math.sqrt(candidates);


        if ((angle < 45 && angle > -45))
        {
            //Debug.Log("Right");
            
            ConnectRoomsHorizontally(leftRoomsChildren, rightRoomsChildren);
            if (leftRoomsChildren.Count + rightRoomsChildren.Count > 8)
            {
                ConnectRoomsHorizontally(leftRoomsChildren, rightRoomsChildren);
            }
        }
        else if ((angle > 45 && angle < 135))
        {
            //Debug.Log("Up");
            ConnectRoomsVertically(leftRoomsChildren, rightRoomsChildren);
            if (leftRoomsChildren.Count > 4 && rightRoomsChildren.Count > 4)
            {
                ConnectRoomsVertically(leftRoomsChildren, rightRoomsChildren);
            }
        }
        else if ((angle > -135 && angle < -45))
        {
            //Debug.Log("Down");
            ConnectRoomsVertically(rightRoomsChildren, leftRoomsChildren);
            if (leftRoomsChildren.Count > 4 && rightRoomsChildren.Count > 4)
            {
                ConnectRoomsVertically(rightRoomsChildren, leftRoomsChildren);
            }
        }
        else
        {
            //Debug.Log("Left");
            
            ConnectRoomsHorizontally(rightRoomsChildren, leftRoomsChildren);

            if(leftRoomsChildren.Count > 4 && rightRoomsChildren.Count > 4)
            {
                ConnectRoomsHorizontally(rightRoomsChildren, leftRoomsChildren);
            }

            
        }
        recentlyConnectedRnodes.Clear();
        r1.connectedWithSibling = true;
        r2.connectedWithSibling = true;

        
        //corridors.Add(c);

    }

    RNode pickLeftCadidate(List<RNode> leftRooms)
    {
        RNode leftCandidate = leftRooms[0];
        RNode leftBackUp = leftRooms[0];
        
        //List<RNode> backUpLeftCandidates = new List<RNode>();
        //backUpLeftCandidates = leftRooms.OrderBy()

        if (leftRooms.Count > 1)
        {
            //get the most left candidate first
            foreach (RNode n in leftRooms)
            {
                if (n.topRight.x < leftCandidate.topRight.x)
                    leftCandidate = n;
            }

            foreach (RNode node in leftRooms)
            {
                if(CheckIfLeftIsPossible(node, leftRooms) == false)
                {
                    recentlyConnectedRnodes.Add(node);
                }
                if (node.topRight.x > leftCandidate.topRight.x && !recentlyConnectedRnodes.Contains(node))
                {
                    leftBackUp = leftCandidate;
                    leftCandidate = node;
                }
            }
        }
        return leftCandidate;
    }
    private bool CheckIfLeftIsPossible(RNode leftCanidate, List<RNode> leftRooms)
    {
        foreach (RNode l in leftRooms)
        {
            if (leftCanidate.topRight.x < l.bottomLeft.x)
            {
                return false;
            }
        }

        return true;
    }
    private bool CheckIfbottomIsPossible(RNode bottomCandidate, List<RNode> bottomRooms)
    {
        foreach (RNode b in bottomRooms)
        {
            if (bottomCandidate.topRight.y < b.bottomLeft.y)
            {
                return false;
            }
        }

        return true;
    }
    private bool CheckIfRightIsPossible(RNode rightCanidate, List<RNode> rightRooms)
    {
        foreach(RNode r in rightRooms)
        {
            if (rightCanidate.bottomLeft.x > r.topRight.x)
            {
                return false;
            }
        }

        return true;
    }
    private bool CheckIfTopIsPossible(RNode topCanidate, List<RNode> topRooms)
    {
        foreach (RNode t in topRooms)
        {
            if (topCanidate.bottomLeft.y > t.topRight.y)
            {
                return false;
            }
        }

        return true;
    }

    void ConnectRoomsHorizontally(List<RNode> leftRooms, List<RNode> rightRooms)
    {
        RNode leftCandidate;
        if(leftRooms.Count == 1)
            leftCandidate = leftRooms[0];
        else
        {
            leftCandidate = pickLeftCadidate(leftRooms);
        }

        //setting a rightside comparison starting candidate
        RNode rightCandidate = rightRooms[0];

        int offsetY = 0;
        bool corridorFailure = false;


        for(int i = 0; i < leftRooms.Count; i++)
        {
            leftCandidate = pickLeftCadidate(leftRooms);

            Vector2 rightSideComparisonPoint = new Vector2(rightCandidate.bottomLeft.x, rightCandidate.bottomLeft.y + rightCandidate.height / 2);
            Vector2 leftSideComparisonPoint = new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2);

            float distance = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);

            //float distance = Math.Abs(test - test1)
            foreach (RNode node in rightRooms)
            {

                rightSideComparisonPoint = new Vector2(node.bottomLeft.x, node.bottomLeft.y + node.height / 2);
                float d = Vector2.Distance(leftSideComparisonPoint, rightSideComparisonPoint);

                if (d < distance && !recentlyConnectedRnodes.Contains(node) && CheckIfRightIsPossible(node, rightRooms))
                {
                    distance = d;
                    rightCandidate = node;
                }
            }

            //PROBLEM IS HERE!
             offsetY = 0;
            if (leftSideComparisonPoint.y + offsetY <= rightCandidate.bottomLeft.y)
            {
                while (leftSideComparisonPoint.y + offsetY <= rightCandidate.bottomLeft.y)
                {
                    offsetY = offsetY + 2;
                    if (leftSideComparisonPoint.y + offsetY + corridorWidth >= leftCandidate.topRight.y)
                    {
                        //Debug.Log("1 Room " + leftCandidate.id + " did not connect well with " + rightCandidate.id);
                        recentlyConnectedRnodes.Add(leftCandidate);
                        corridorFailure = true;
                        break;
                    }
                }
            }
            else if (leftSideComparisonPoint.y + offsetY + corridorWidth >= rightCandidate.topLeft.y)
            {
                while (leftSideComparisonPoint.y + offsetY + corridorWidth >= rightCandidate.topLeft.y)
                {
                    offsetY = offsetY - 2;
                    if (leftSideComparisonPoint.y + offsetY <= leftCandidate.bottomRight.y)
                    {
                        //Debug.Log("1 Room " + leftCandidate.id + " did not connect well with " + rightCandidate.id);
                        corridorFailure = true;
                        recentlyConnectedRnodes.Add(leftCandidate);
                        break;
                    }
                }
            }
            if (corridorFailure == false)
            {
                break;
            }
        }
        //Debug.Log("Used left candidate: " + leftCandidate.id + " and right candidate: " + rightCandidate.id);


        CNode c = new CNode(new Vector2(leftCandidate.bottomRight.x, leftCandidate.bottomRight.y + leftCandidate.height / 2 + offsetY), 
            new Vector2 (rightCandidate.topLeft.x , leftCandidate.bottomRight.y + leftCandidate.height / 2 + offsetY + corridorWidth), 
            10, idTracker++);

        c.vertical = false;
        corridors.Add(c);

        Doorway ldr = new Doorway(c.bottomLeft, new Vector2(c.bottomLeft.x, c.topRight.y), true);
        leftCandidate.doorways.Add(ldr);
        Doorway rdr = new Doorway(new Vector2(c.topRight.x, c.bottomLeft.y), c.topRight, true);
        rightCandidate.doorways.Add(rdr);

        AddCorridorObject(ldr.pillarOne, pillar, Vector3.zero);
        AddCorridorObject(ldr.pillarTwo, pillar, Vector3.zero);
        AddCorridorObject(rdr.pillarOne, pillar, Vector3.zero);
        AddCorridorObject(rdr.pillarTwo, pillar, Vector3.zero);

        float buildPos = c.bottomLeft.x;
        while (buildPos < c.topRight.x)
        {
            if(buildPos + 4.5 < c.topRight.x)
            {
                AddCorridorObject(new Vector2(buildPos + 2.5f , c.topRight.y), Wall5, Vector3.zero);
                AddCorridorObject(new Vector2(buildPos + 2.5f, c.bottomLeft.y), Wall5, new Vector3(0, 180, 0));
                buildPos += 5;
            }
            else if (buildPos + 0.5 < c.topRight.x)
            {
                AddCorridorObject(new Vector2(buildPos + 0.5f, c.topRight.y), Wall1, Vector3.zero);
                AddCorridorObject(new Vector2(buildPos + 0.5f, c.bottomLeft.y), Wall1, new Vector3(0, 180, 0));
                buildPos += 1;
            }
            else
            {
                break;
            }
        }
        recentlyConnectedRnodes.Add(leftCandidate);
        recentlyConnectedRnodes.Add(rightCandidate);
    }
    
    private RNode pickBottomCandidate(List<RNode> bottomRooms)
    {
        RNode bottomCandidate = bottomRooms[0];

        if (bottomRooms.Count > 1)
        {
            foreach (RNode rnode in bottomRooms)
            {
                if (rnode.topRight.y < bottomCandidate.topRight.y)
                    bottomCandidate = rnode;
            }
            foreach (RNode node in bottomRooms)
            {
                if (CheckIfbottomIsPossible(node, bottomRooms) == false)
                {
                    recentlyConnectedRnodes.Add(node);
                }
                if (node.topRight.y > bottomCandidate.topRight.y && !recentlyConnectedRnodes.Contains(node))
                {
                    bottomCandidate = node;
                }
            }
        }

        return bottomCandidate;
    }
    void ConnectRoomsVertically(List<RNode> bottomRooms, List<RNode> topRooms)
    {

        RNode bottomCandidate; // = bottomRooms[0];
        RNode topCandidate = topRooms[0];


        int offsetX = 0;
        bool connectionFailure = false;

        bottomCandidate = pickBottomCandidate(bottomRooms);

        for(int i = 0; i < bottomRooms.Count; i++)
        {
            bottomCandidate = pickBottomCandidate(bottomRooms);
            //point a low and center on width of a room above
            Vector2 aboveComparisonPoint = new Vector2(topCandidate.bottomLeft.x + topCandidate.width / 2, topCandidate.bottomLeft.y);
            Vector2 belowComparisonPoint = new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2, bottomCandidate.topRight.y);

            float distance = Vector2.Distance(belowComparisonPoint, aboveComparisonPoint);

            foreach (RNode node in topRooms)
            {

                aboveComparisonPoint = new Vector2(node.bottomLeft.x + node.width / 2, node.bottomLeft.y);
                float d = Vector2.Distance(belowComparisonPoint, aboveComparisonPoint);

                if (d < distance && !recentlyConnectedRnodes.Contains(node) && CheckIfTopIsPossible(node, topRooms))
                {
                    distance = d;
                    topCandidate = node;
                }
            }

            offsetX = 0;

            if (belowComparisonPoint.x + offsetX <= topCandidate.bottomLeft.x)
            {
                while (belowComparisonPoint.x + offsetX <= topCandidate.bottomLeft.x)
                {
                    offsetX = offsetX + 2;
                    if (belowComparisonPoint.x + offsetX + corridorWidth >= bottomCandidate.topRight.x)
                    {
                        //Debug.Log("2 Room " + bottomCandidate.id + " did not connect well with " + topCandidate.id);
                        connectionFailure = true;
                        recentlyConnectedRnodes.Add(bottomCandidate);
                        break;
                    }
                }
            }
            else if (belowComparisonPoint.x + offsetX + corridorWidth >= topCandidate.bottomRight.x)
            {
                while (belowComparisonPoint.x + offsetX + corridorWidth >= topCandidate.bottomRight.x)
                {
                    offsetX = offsetX - 2;

                    if (belowComparisonPoint.x + offsetX <= bottomCandidate.topLeft.x)
                    {
                        //Debug.Log("2 Room " + bottomCandidate.id + " did not connect well with " + topCandidate.id);
                        connectionFailure = true;
                        recentlyConnectedRnodes.Add(bottomCandidate);
                        break;
                    }

                }
            }

            if (connectionFailure == false)
            {
                
                break;
            }
        }
        //Debug.Log("Used bottom candidate: " + bottomCandidate.id + " and top candidate: " + topCandidate.id);

        CNode c = new CNode(new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2 + offsetX, bottomCandidate.topRight.y), 
            new Vector2(bottomCandidate.bottomLeft.x + bottomCandidate.width / 2 + offsetX + corridorWidth, topCandidate.bottomLeft.y), 
            10, idTracker++);


        c.vertical = true;
        corridors.Add(c);

        Doorway bdr = new Doorway(c.bottomLeft, new Vector2(c.topRight.x, c.bottomLeft.y), false);
        bottomCandidate.doorways.Add(bdr);
        Doorway tdr = new Doorway(new Vector2(c.bottomLeft.x, c.topRight.y), c.topRight, false);
        topCandidate.doorways.Add(tdr);

       
       AddCorridorObject(bdr.pillarOne, pillar, Vector3.zero);
       AddCorridorObject(bdr.pillarTwo, pillar, Vector3.zero);
       AddCorridorObject(tdr.pillarOne, pillar, Vector3.zero);
       AddCorridorObject(tdr.pillarTwo, pillar, Vector3.zero);

        float buildPos = c.bottomLeft.y;
        while (buildPos < c.topRight.y)
        {
            if (buildPos + 4.5 < c.topRight.y)
            {
                AddCorridorObject(new Vector2(c.topRight.x, buildPos + 2.5f), Wall5, new Vector3(0, 90, 0));
                AddCorridorObject(new Vector2(c.bottomLeft.x, buildPos + 2.5f), Wall5, new Vector3(0, 270, 0));
                buildPos += 5;
            }
            else if (buildPos + 0.5 < c.topRight.y)
            {
                AddCorridorObject(new Vector2(c.topRight.x, buildPos + 0.5f), Wall1, new Vector3(0, 90, 0));
                AddCorridorObject(new Vector2(c.bottomLeft.x, buildPos + 0.5f), Wall1, new Vector3(0, 270, 0));
                buildPos += 1;
            }
            else
            {
                break;
            }
        }
        recentlyConnectedRnodes.Add(bottomCandidate);
        recentlyConnectedRnodes.Add(topCandidate);
    }

    void AddCorridorObject(Vector2 pos, GameObject type, Vector3 rotation)
    {
        //Vector3 rotation = new Vector3(0, 0, 0);
        PCGObjects obj = new PCGObjects(pos, type, rotation);
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