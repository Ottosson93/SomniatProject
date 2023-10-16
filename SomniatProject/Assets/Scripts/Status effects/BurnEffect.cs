using UnityEngine;
using UnityEngine.UIElements;

public class BurnEffect : MonoBehaviour
{
    public float duration = 2.0f;
    public int damagePerTick = 10;
    private float tickInterval = 1.0f;

    private float elapsedTime = 0.0f;
    private float timeSinceLastTick = 0.0f;

    public ParticleSystem fireParticles;

    private bool playFireParticles = false;

    public void SetParticleSystem(ParticleSystem particleSystem)
    {
        fireParticles = particleSystem;
        playFireParticles = true;
    }
    private void Start()
    {
        if(playFireParticles && fireParticles != null)
        {
            PlayFireParticles();
            playFireParticles=false;
            Debug.Log("Playing fire particles, name of prefab: " + fireParticles.name);
        }
    }


    private void OnDestroy()
    {
        StopFireParticles();
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

    private void PlayFireParticles()
    {
        if(fireParticles != null)
        {
            fireParticles.transform.position = transform.position;
            fireParticles.Play();
            Debug.Log("Playing fire particles");
        }
    }


    private void StopFireParticles()
    {
        if(fireParticles != null)
        {
            fireParticles.Stop();
            Debug.Log("Stopped fire particles.");
        }
    }

}
