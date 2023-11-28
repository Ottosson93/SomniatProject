using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundEvents : MonoBehaviour
{
    [field: SerializeField] public EventReference death { get; private set; }
    [field: SerializeField] public EventReference dash { get; private set; }
    [field: SerializeField] public EventReference meleeAttack { get; private set; }
    [field: SerializeField] public EventReference rangedAttack { get; private set; }
    [field: SerializeField] public EventReference bossMelee { get; private set; }
    [field: SerializeField] public EventReference bossThrow{ get; private set; }
    [field: SerializeField] public EventReference potion { get; private set; }
    [field: SerializeField] public EventReference boxCollapse { get; private set; }
    [field: SerializeField] public EventReference meleeGruntAttack { get; private set; }
    [field: SerializeField] public EventReference rangedGruntAttack { get; private set; }
    [field: SerializeField] public EventReference fireball { get; private set; }
    [field: SerializeField] public EventReference piercingArrow { get; private set; }
    [field: SerializeField] public EventReference berzerk { get; private set; }
    [field: SerializeField] public EventReference music { get; private set; }
    [field: SerializeField] public EventReference bossMusic { get; private set; }
    [field: SerializeField] public EventReference idleMusic { get; private set; }
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
