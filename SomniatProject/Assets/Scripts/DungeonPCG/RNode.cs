using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;

#endif

using UnityEngine;

public class RNode
{
    public float height, width;
    public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    public Vector3 centerPos; 
    public RNode parent, sibling;
    public List<RNode> children = new List<RNode>();
    public bool manual = false;
    public bool bottom = false;
    public bool vertical = false;
    public bool connectedWithSibling = false;

    public bool isGreenRoom = false;
    public bool isOrangeRoom = false;
    public bool isRedRoom = false;

    public List<Doorway> doorways = new List<Doorway>();

    public int id;
    

    public RNode(Vector2 bottomLeft, Vector2 topRight, int id) //Vector2 topLeft, Vector2 bottomRight
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;

        this.id = id; 

        if (bottomLeft.x < 0)
        {
            if(topLeft.x < 0)
            {
                this.width = bottomLeft.x * -1 + topRight.x * -1;
            }
            else
                this.width = bottomLeft.x * -1 + topRight.x;
        }
        else
            this.width = topRight.x - bottomLeft.x;
        if (bottomLeft.y < 0)
        {
            if(topLeft.y < 0)
            {
                this.height = bottomLeft.y * -1 + topRight.y * -1;
            }
            else
                this.height = bottomLeft.y * -1 + topRight.y;
        }
        else
            this.height = topRight.y - bottomLeft.y;

        //this.height = topLeft.y + bottomLeft.y; // not Correct
        //this.width = bottomLeft.x + topRight.x; // not Correct
        //this.childOne = childOne;
        //this.childTwo = childTwo;

        centerPos.x = this.bottomLeft.x + this.width / 2;
        centerPos.y = this.bottomLeft.y + this.height / 2;

    }

    public void UpdateCorners()
    {
        bottomRight = new Vector2(topRight.x, bottomLeft.y);
        topLeft = new Vector2(bottomLeft.x, topRight.y);
    }

    public void UpdateWH()
    {
        if (bottomLeft.x < 0)
        {
            if (topLeft.x < 0)
            {
                width = bottomLeft.x * -1 + topRight.x * -1;
            }
            else
                width = bottomLeft.x * -1 + topRight.x;
        }
        else
            width = topRight.x - bottomLeft.x;
        if (bottomLeft.y < 0)
        {
            if (topLeft.y < 0)
            {
                height = bottomLeft.y * -1 + topRight.y * -1;
            }
            else
                height = bottomLeft.y * -1 + topRight.y;
        }
        else
            height = topRight.y - bottomLeft.y;

        centerPos.x = bottomLeft.x + width / 2;
        centerPos.y = bottomLeft.y + height / 2;
    }
}