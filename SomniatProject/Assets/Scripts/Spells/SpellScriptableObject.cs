using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellScriptableObject : ScriptableObject
{
    [Header("Damage")]
    public int DamageAmount = 10;
    public int DamageIncrease = 2;

    [Header("Lucidity")]
    public float LucidityCost = 5f;
    public float Lifetime = 2f;

    [Header("General Spell Info")]
    public float Speed = 15f;
    public float SpellRadius;
    public float RotationSpeed = 100f;

    [Header("Prefabs")]
    public GameObject ExplosionPrefab;

    [Header("Durations")]
    public float ExplosionDuration = 2f;
    public float ChillDuration = 2f;
}
