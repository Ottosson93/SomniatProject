using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RNode
{
    public float height, width;
    public Vector2 topLeft, topRight, bottomLeft, bottomRight;  
    public RNode parent, sibling;
    public bool bottom = false;
    public bool vertical = false;
    

    public RNode(Vector2 bottomLeft, Vector2 topRight) //Vector2 topLeft, Vector2 bottomRight
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;

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
    }
}