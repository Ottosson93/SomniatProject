using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SocialPlatforms.GameCenter;
public class PCGObjects
{
    public GameObject objectType;
    public Vector3 spawnPoint;

    public PCGObjects(Vector2 centerPoint, GameObject obj)
    {
        this.spawnPoint = new Vector3(centerPoint.x, 0, centerPoint.y);
        this.objectType = obj;
    }
}