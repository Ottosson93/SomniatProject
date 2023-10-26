using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    private ParticleSystem burningParticleEffect;
    private float burnDuration;
    private int damagePerTick;
    private float tickInterval;
    private float elapsedTime;
    private float timeSinceLastTick;

    public void Initialize(float duration, ParticleSystem burnParticleEffect, int damagePerTick, float tickInterval)
    {
        burnDuration = duration;
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;

        if (burnParticleEffect != null)
        {
            burningParticleEffect = Instantiate(burnParticleEffect, transform);
            burningParticleEffect.transform.localPosition = Vector3.zero;
            burningParticleEffect.Play();
        }

        // Schedule the end of the burn effect
        Invoke("EndBurnEffect", burnDuration);
    }

    private void Update()
    {
        if (burningParticleEffect == null)
        {
            return; // Particle system is missing; exit.
        }

        elapsedTime += Time.deltaTime;

        timeSinceLastTick += Time.deltaTime;
        if (timeSinceLastTick >= tickInterval)
        {
            ApplyBurnDamage();
            timeSinceLastTick = 0.0f;
        }

        if (elapsedTime >= burnDuration)
        {
            EndBurnEffect();
        }
    }

    private void ApplyBurnDamage()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damagePerTick);
        }
        else if (TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damagePerTick);
        }
    }

    private void EndBurnEffect()
    {
        if (burningParticleEffect != null)
        {
            burningParticleEffect.Stop();
        }

        Destroy(burningParticleEffect.gameObject);

        // Implement any additional logic to end the burn effect here.

        Destroy(this);
    }

    private new bool TryGetComponent<T>(out T component) where T : Component
    {
        component = GetComponent<T>();
        return component != null;
    }
}
