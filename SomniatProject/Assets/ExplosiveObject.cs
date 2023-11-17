using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public LayerMask targetLayer;
    public int currentHealth, health;
    public SpellScriptableObject SpellToCast;
    public Collider[] explosionColliders;

    private void Start()
    {
        health = 5;
        currentHealth = health;
        explosionColliders = new Collider[10];
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        CreateExplosionEffect();
        DealDamageInRadius();
    }

    private void CreateExplosionEffect()
    {
        GameObject explosion = Instantiate(SpellToCast.ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, SpellToCast.ExplosionDuration);
    }

    

    private void DealDamageInRadius()
    {
        int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, SpellToCast.SpellRadius * 6, explosionColliders);

        if (overlapCount > 0)
        {
            for (var overlapIndex = 0; overlapIndex < overlapCount; overlapIndex++)
            {
                Collider hitCollider = explosionColliders[overlapIndex];
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
                //else if (gameObject != null)
                //{
                //    TakeDamage(SpellToCast.DamageAmount);
                //}
            }
        }

        //Collider[] overlapCount = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius * 2);

        //foreach (Collider hitCollider in overlapCount)
        //{
        //    Enemy enemy = hitCollider.GetComponent<Enemy>();
        //    Player player = hitCollider.GetComponent<Player>();

        //    if (enemy != null)
        //    {
        //        enemy.TakeDamage(SpellToCast.DamageAmount);
        //    }
        //    else if (player != null)
        //    {
        //        player.TakeDamage(SpellToCast.DamageAmount);
        //    }
        //}
    }
}
