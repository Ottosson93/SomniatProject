using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    private SphereCollider myCollider;
    private Rigidbody myRigidbody;

    [SerializeField] private GameObject hitParticleEffectPrefab;

    private int damageMultiplier = 1;


    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;


        Destroy(this.gameObject, SpellToCast.Lifetime);
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
            if (SpellToCast.name.Equals("Fireball"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
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
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(SpellToCast.DamageAmount * damageMultiplier);
                    damageMultiplier += SpellToCast.DamageIncrease;


                    InstantiateHitParticleEffect(other.transform.position);
                }
            }

        }
    }

    private void InstantiateHitParticleEffect(Vector3 hitPos)
    {
        if (hitParticleEffectPrefab != null)
        {
            GameObject chillParticleEffect = Instantiate(hitParticleEffectPrefab, hitPos, Quaternion.identity);

            Destroy(chillParticleEffect.gameObject, SpellToCast.ChillDuration);
        }
    }

    private void DealDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius*2);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
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

                }

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
