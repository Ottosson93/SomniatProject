using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
#if UNITY_EDITOR

using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class PCGObjects
{
    public GameObject objectType;
    public Vector3 spawnPoint;
    public Vector3 rotation;
    public Quaternion angle; 

    public PCGObjects(Vector2 centerPoint, GameObject obj, Vector3 rot)
    {
        this.spawnPoint = new Vector3(centerPoint.x, 0, centerPoint.y);
        this.objectType = obj;
        this.rotation = rot;
        this.angle = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
}