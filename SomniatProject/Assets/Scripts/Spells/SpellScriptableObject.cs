using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellScriptableObject : ScriptableObject
{
    [Header("Damage")]
    public int DamageAmount = 10;
    public int DamagePerTick = 5;

    [Header("Lucidity")]
    public float LucidityCost = 5f;
    public float Lifetime = 2f;

    [Header("General Spell Info")]
    public float Speed = 15f;
    public float SpellRadius;
    public float RotationSpeed = 100f;

    [Header("Prefabs & Particle Systems")]
    public GameObject ExplosionPrefab;
    public ParticleSystem LightningImpact;
    public ParticleSystem LightningStun;
    public ParticleSystem BurnParticleSystem;
    public ParticleSystem BerserkParticleSystem;

    [Header("Durations")]
    public float ExplosionDuration = 2f;
    public float StunDuration = 2f;
    public float BurnDuration = 5f;
    public float TickInterval = 0.75f;

    [Header("Character Stats")]
    public float DamageBoost;
    public float AttackSpeedBoost;
    public float MovementSpeedBoost;
    public float ArmorReduction;

    [Header("Other")]
    public bool IsInstantiatedByAbilityCast = true;
}
