using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMadeRoom
{
    int width, height;
    public Vector2 bottomLeftCorner, topRightCorner;
    public Vector3 centerPos; 
    public GameObject preMadeRoom;

    public PreMadeRoom(Vector3 centerPos, GameObject pmr)
    {
        this.centerPos = centerPos;
        this.preMadeRoom = pmr;
        //this.width = width;
        //this.height = height;
    }
}