using UnityEngine;
using UnityEngine.AI;

public class StunEffect : MonoBehaviour
{
    private ParticleSystem lightningStunParticleEffect;
    private float stunDuration;
    private NavMeshAgent navMeshAgent;
    private Vector3 originalDestination;
    private Animator animator;

    public void Initialize(float duration, ParticleSystem stunParticleEffect, Animator enemyAnimator)
    {
        stunDuration = duration;
        animator = enemyAnimator;

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

            if(animator != null)
            {
                animator.SetTrigger("StunTrigger");
            }
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

            if(animator != null)
            {
                animator.ResetTrigger("StunTrigger");
            }
        }


        Destroy(this);
    }
}
