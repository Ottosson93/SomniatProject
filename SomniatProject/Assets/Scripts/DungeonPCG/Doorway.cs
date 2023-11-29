using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class Doorway
{
    public bool vertical;

    //if vertical, then pillarONe will always be the lower one otherwise it will be the left one.
    public Vector2 pillarOne;
    public Vector2 pillarTwo;

    public Doorway( Vector2 pillarOne, Vector2 pillarTwo, bool v)
    {
        this.pillarOne = pillarOne;
        this.pillarTwo = pillarTwo;
        this.vertical = v;
    }
}