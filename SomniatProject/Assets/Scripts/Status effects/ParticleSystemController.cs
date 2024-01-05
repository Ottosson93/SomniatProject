using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem swordSlashParticles;
    private Animator animator;

    void Start()
    {
        swordSlashParticles = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();

        if (swordSlashParticles == null)
        {
            Debug.LogError("Particle System component not found!");
        }
    }

    void Update()
    {
        bool isAttacking = animator.GetBool("Attack");

        if (isAttacking)
        {
            PlayParticleSystem();
        }
        else
        {
            StopParticleSystem();
        }
    }

    void PlayParticleSystem()
    {
        if (swordSlashParticles != null)
        {
            if (!swordSlashParticles.isPlaying)
            {
                swordSlashParticles.Play();
            }
        }
    }

    void StopParticleSystem()
    {
        if (swordSlashParticles != null)
        {
            if (swordSlashParticles.isPlaying)
            {
                swordSlashParticles.Stop();
            }
        }
    }
}

