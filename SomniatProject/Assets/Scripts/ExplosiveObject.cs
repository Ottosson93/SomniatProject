using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public int health, explosionDamage, explosionDamageToPlayer;
    public SpellScriptableObject SpellToCast;
    public Collider[] explosionColliders;

    private void Start()
    {
        explosionColliders = new Collider[10];
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject.transform.parent.gameObject);
        //Can change to this if you want lucid objects to stay after destruction
        //Destroy(gameObject);
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
        //Debug.Log("overlapCount.Length " + overlapCount);

        for (var overlapIndex = 0; overlapIndex < overlapCount; overlapIndex++)
        {
            Collider hitCollider = explosionColliders[overlapIndex];
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            Player player = hitCollider.GetComponent<Player>();
            ExplosiveObject explosiveObject = hitCollider.GetComponent<ExplosiveObject>();

            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
            else if (player != null)
            {
                player.TakeDamage(explosionDamageToPlayer);
            }
            //else if (gameObject != null)
            //{
            //    TakeDamage(SpellToCast.DamageAmount);
            //}
        }
    }
}
