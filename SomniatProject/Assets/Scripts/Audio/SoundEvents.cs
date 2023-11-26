using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundEvents : MonoBehaviour
{
    [field: SerializeField] public EventReference death { get; private set; }
    [field: SerializeField] public EventReference dash { get; private set; }
    [field: SerializeField] public EventReference hit { get; private set; }
    [field: SerializeField] public EventReference music { get; private set; }
    public static SoundEvents instance { get; private set; }
    

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one 'Sound Events' script in the scene");
        }
        instance = this;
    }
}
