using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    public float duration = 5.0f;
    public int damagePerTick = 10;
    private float tickInterval = 1.0f;

    private float elapsedTime = 0.0f;
    private float timeSinceLastTick = 0.0f;

    public ParticleSystem fireParticles;

    public void SetParticleSystem(ParticleSystem particleSystem)
    {
        fireParticles = particleSystem;
    }
    private void Start()
    {
        if(fireParticles != null)
        {
            fireParticles.Play();
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        timeSinceLastTick += Time.deltaTime;
        if (timeSinceLastTick >= tickInterval)
        {
            Player player = GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damagePerTick);
            }
            Enemy enemy = GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damagePerTick);
            }

            timeSinceLastTick = 0.0f;
        }

        if (elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }

}
