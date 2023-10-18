using UnityEngine;
using UnityEngine.UIElements;

public class BurnEffect : MonoBehaviour
{
    public float duration = 5.0f;
    public int damagePerTick = 5;
    private float tickInterval = 0.75f;

    private float elapsedTime = 0.0f;
    private float timeSinceLastTick = 0.0f;
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
