using BehaviorTree;
using System.Diagnostics;

using UnityEngine;
using TMPro;

public class CNode
{
    private int corridorWidth;
    public Vector2 bottomLeft, topRight;
    private RNode room1, room2;
    private bool vertical; 

    public CNode(Vector2 bottomLeft, Vector2 topRight, int width)
    {
        corridorWidth = width;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }

}