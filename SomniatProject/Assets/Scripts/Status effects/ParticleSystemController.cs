using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem swordSlashParticles;

    void Start()
    {
        swordSlashParticles = GetComponent<ParticleSystem>();

        if (swordSlashParticles == null)
        {
            Debug.LogError("Particle System component not found!");
        }
    }

    void Update()
    {
        bool isAttacking = YourAttackCheckLogic();

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

    bool YourAttackCheckLogic()
    {
        
        return Input.GetButtonDown("Fire1");
    }
}

