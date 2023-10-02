using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellScriptableObject : ScriptableObject
{
    public float DamageAmount = 10f;
    public float LucidityCost = 5f;
    public float Lifetime = 2f;
    public float Speed = 15f;
    public float SpellRadius;
    public GameObject ExplosionPrefab;
    public float ExplosionDuration = 2f;

   
}
