using UnityEngine;
using UnityEngine.AI;

public class StunEffect : MonoBehaviour
{
    private ParticleSystem lightningStunParticleEffect;
    private float stunDuration;
    private NavMeshAgent navMeshAgent;
    private Vector3 originalDestination;


    public void Initialize(float duration, ParticleSystem stunParticleEffect)
    {
        stunDuration = duration;

        if (stunParticleEffect != null)
        {
            lightningStunParticleEffect = Instantiate(stunParticleEffect, transform);
            lightningStunParticleEffect.transform.localPosition = Vector3.zero;
            lightningStunParticleEffect.Play();
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent != null )
        {
            originalDestination = navMeshAgent.destination;
            navMeshAgent.isStopped = true;
        }



        Invoke("EndStunEffect", stunDuration);
    }

    private void EndStunEffect()
    {
        if (lightningStunParticleEffect != null)
        {
            lightningStunParticleEffect.Stop();
        }

        
        Destroy(lightningStunParticleEffect.gameObject);

        if(navMeshAgent != null )
        {
            navMeshAgent.destination = originalDestination;
            navMeshAgent.isStopped= false;
        }


        Destroy(this);
    }
}
