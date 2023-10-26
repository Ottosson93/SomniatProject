using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    private SphereCollider myCollider;
    private Rigidbody myRigidbody;

    private ParticleSystem lightningImpactParticleEffect;



    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;


        Destroy(this.gameObject, SpellToCast.Lifetime);

        if (SpellToCast.LightningImpact != null)
        {
            lightningImpactParticleEffect = Instantiate(SpellToCast.LightningImpact, transform.position, Quaternion.identity);
            lightningImpactParticleEffect.Stop();
        }
    }

    private void Update()
    {
        if (SpellToCast.Speed > 0)
        {
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
            if (SpellToCast.name.Equals("Piercing Arrow"))
            {
                transform.Rotate(0f, 0f, SpellToCast.RotationSpeed * Time.deltaTime,  Space.Self);

               
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon"))
        {
            Physics.IgnoreCollision(myCollider, other);
        }
        else
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (SpellToCast.name.Equals("Fireball"))
            {
                if (enemy != null)
                {
                    BurnEffect burnEffect = other.gameObject.GetComponent<BurnEffect>();
                    if (burnEffect == null)
                    {
                        burnEffect = other.gameObject.AddComponent<BurnEffect>();
                    }
                }
                DealDamageInRadius();
                CreateExplosionEffect();
            }
            else if (SpellToCast.name.Equals("Piercing Arrow"))
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(SpellToCast.DamageAmount);
                    PlayLightningImpactAtEnemyPosition(enemy.transform.position);

                    StunEffect stunEffect = enemy.gameObject.AddComponent<StunEffect>();
                    stunEffect.Initialize(SpellToCast.StunDuration, SpellToCast.LightningStun);

                }
            }

        }
    }

    private void PlayLightningImpactAtEnemyPosition(Vector3 position)
    {
        if (lightningImpactParticleEffect != null)
        {
            lightningImpactParticleEffect.transform.position = position;
            lightningImpactParticleEffect.Play();
            lightningImpactParticleEffect.Stop();
        }
    }

    private void DealDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius*2);

        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(SpellToCast.DamageAmount);

                BurnEffect burnEffect = hitCollider.gameObject.GetComponent<BurnEffect>();
                if (burnEffect == null)
                {
                    burnEffect = hitCollider.gameObject.AddComponent<BurnEffect>();
                }

                burnEffect.Initialize(SpellToCast.BurnDuration, SpellToCast.BurnParticleSystem, SpellToCast.DamagePerTick, SpellToCast.TickInterval);
            }
        }

        Destroy(this.gameObject);
    }

    private void CreateExplosionEffect()
    {
        GameObject explosion = Instantiate(SpellToCast.ExplosionPrefab, transform.position, Quaternion.identity);

        explosion.transform.localScale = new Vector3(SpellToCast.SpellRadius * 2f, SpellToCast.SpellRadius * 2f, SpellToCast.SpellRadius * 2f);

        Destroy(explosion, SpellToCast.ExplosionDuration);
    }
}
