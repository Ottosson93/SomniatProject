using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public LayerMask enemyLayer;
    public int currentHealth, health = 5;
    public DamageTextPlayer damageTextPlayer;
    public SpellScriptableObject SpellToCast;
    public GameObject explosionEffect;
    public Collider[] colliders;

    private void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageTextPlayer.SubtractHealth(damage, transform);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        DealDamageInRadius();
        Destroy(explosionEffect, 1f);
        
    }

    private void DealDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius * 2);

        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            Player player = hitCollider.GetComponent<Player>();
            ExplosiveObject explosion = hitCollider.GetComponent<ExplosiveObject>();

            if (enemy != null)
            {
                enemy.TakeDamage(SpellToCast.DamageAmount);
            }
            else if (player != null)
            {
                player.TakeDamage(SpellToCast.DamageAmount);
            }
            else if (gameObject != null && hitCollider.gameObject != gameObject)
            {
                TakeDamage(SpellToCast.DamageAmount);
            }
        }
    }
}
