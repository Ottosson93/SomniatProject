using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellScriptableObject : ScriptableObject
{
    public int DamageAmount = 10;
    public float LucidityCost = 5f;
    public float Lifetime = 2f;
    public float Speed = 15f;
    public float SpellRadius;
    public GameObject ExplosionPrefab;
    public float ExplosionDuration = 2f;
    public int BurnDamagePerTick = 10;
    public float BurnDuration = 5.0f;

   
}
