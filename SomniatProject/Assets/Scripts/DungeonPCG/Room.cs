using System.Collections.Generic;
#if UNITY_EDITOR
using TreeEditor;
#endif
using UnityEngine;

public class Room
{
    public Vector2 width, height;
    public Vector2 bottomLeft, topRight, bottomRight, topLeft;
    public Room parent; 
    public bool isLeaf;
    public List<Room> children;

    public Room(Vector2 bottomLeft, Vector2 topRight, bool isLeaf, Room parent)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        bottomRight = new Vector2(topRight.x, bottomLeft.x);
        topLeft = new Vector2(bottomLeft.x, topRight.y);

        this.parent = parent;
        this.isLeaf = isLeaf;
        children = new List<Room>();
    }
}