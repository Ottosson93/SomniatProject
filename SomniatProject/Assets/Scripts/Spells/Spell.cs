using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;
    private Collider[] spellColliders = new Collider[5];
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;

    private ParticleSystem lightningImpactParticleEffect;

    private Player player;
    private bool berserkApplied;
    
    private ParticleSystem berserkParticles;

    private Vector3 playerPos;

    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;
    }


    private void Awake()
    {
        player = FindObjectOfType<Player>();

     //  myCollider = GetComponent<SphereCollider>();
     //  myCollider.isTrigger = true;
     //  myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        

        if (!SpellToCast.name.Equals("Berserk"))
        {
            Destroy(this.gameObject, SpellToCast.Lifetime);
        }


        //if (SpellToCast.LightningImpact != null)
        //{
        //    lightningImpactParticleEffect = Instantiate(SpellToCast.LightningImpact, transform.position, Quaternion.identity);
        //    lightningImpactParticleEffect.Stop();
        //}

        if (SpellToCast.name.Equals("Berserk"))
        {
            berserkApplied = false;
            player.EndBerserk();
            Debug.Log("original Speed: " + player.speed + " original Attack Speed: " + player.attackSpeed + " original Damage Amount: " + player.meleeDamage + " original Armor Amount: " + player.damageReduction);
            player.StartBerserk(SpellToCast.ArmorReduction, SpellToCast.AttackSpeedBoost, SpellToCast.DamageBoost, SpellToCast.MovementSpeedBoost);
            Debug.Log("new Speed: " + player.speed + " new Attack Speed: " + player.attackSpeed + " new Damage Amount: " + player.meleeDamage + " new Armor Amount: " + player.damageReduction);
            StartCoroutine(ApplyBerserkEffects());
        }
    }

    private void Update()
    {
        if (SpellToCast.Speed > 0 && (SpellToCast.name.Equals("Piercing Arrow") || SpellToCast.name.Equals("Fireball")) || SpellToCast.name.Equals("Boulder"))
        {
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
            if (SpellToCast.name.Equals("Piercing Arrow"))
            {
                transform.Rotate(0f, 0f, SpellToCast.RotationSpeed * Time.deltaTime,  Space.Self);            
            }
        }

        if(player != null)
        {
            if(berserkParticles != null && SpellToCast.Lifetime >= 0)
            {
                berserkParticles.transform.position = player.transform.position;
                playerPos = player.transform.position;
            }
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LucidCapsule"))
            Debug.Log("SpellFired Exit");

        Destroy(this.gameObject);
                //Debug.Log("Exiting");
            //Physics.IgnoreCollision(myCollider, other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("LucidCapsule") || other.gameObject.CompareTag("Weapon"))
        {
            Physics.IgnoreCollision(myCollider, other);
            Debug.Log("SpellFired Enter");
        }
        else
        {
            Enemy enemy = other.GetComponent<Enemy>();
            ExplosiveObject explosiveObject = other.GetComponent<ExplosiveObject>();
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
                    enemy.TakeDamage(SpellToCast.DamageAmount + player.CalculateSpellDamageModifierFromRelics());
                    PlayLightningImpactAtEnemyPosition(enemy.transform.position);

                    StunEffect stunEffect = enemy.gameObject.AddComponent<StunEffect>();
                    stunEffect.Initialize(SpellToCast.StunDuration, SpellToCast.LightningStun, enemy.GetComponent<Animator>());

                }

                if (explosiveObject != null)
                {
                    explosiveObject.TakeDamage(SpellToCast.DamageAmount);
                }
            }


            if (other.gameObject.CompareTag("Boss") && transform.gameObject.CompareTag("Boulder") || other.gameObject.CompareTag("Enemy") && transform.gameObject.CompareTag("Boulder"))
            {
                Physics.IgnoreCollision(myCollider, enemy.GetComponent<Collider>());
            }


        }

        if (SpellToCast.name.Equals("Boulder"))
        {
            player = other.GetComponent<Player>();

            if (player != null)
            {
                BurnEffect burnEffect = player.gameObject.GetComponent<BurnEffect>();
                if (burnEffect == null)
                {
                    burnEffect = player.gameObject.AddComponent<BurnEffect>();
                }
            }
            BossDealDamageInRadius();
            CreateExplosionEffect();
        }


    }

    private void PlayLightningImpactAtEnemyPosition(Vector3 position)
    {
        if (lightningImpactParticleEffect != null)
        {
            lightningImpactParticleEffect.transform.position = position;
            lightningImpactParticleEffect.Play();
            lightningImpactParticleEffect.Stop();
            Destroy(lightningImpactParticleEffect);
        }
    }

    private IEnumerator ApplyBerserkEffects()
    {
        if (berserkApplied)
            yield break;
        
        if(player != null)
        {
            berserkParticles = Instantiate(SpellToCast.BerserkParticleSystem, player.transform);
            //Berserk SFX
            AudioManager.instance.PlaySingleSFX(SoundEvents.instance.berzerk, player.transform.position);
            berserkParticles.Play();

            yield return new WaitForSeconds(SpellToCast.Lifetime);

            if(berserkParticles != null)
            {
                berserkParticles.Stop();
                Destroy(berserkParticles.gameObject);
            }

            //reset stats
            player.EndBerserk();
            Debug.Log("reset Speed: " + player.speed + " reset Attack Speed: " + player.attackSpeed + " reset Damage Amount: " + player.meleeDamage + " reset Armor Amount: " + player.damageReduction);
        }


        berserkApplied = true;

        Destroy(this.gameObject);
    }

    private void DealDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius*6);
        
        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            ExplosiveObject explosiveObject = hitCollider.GetComponent<ExplosiveObject>();
            
            if (explosiveObject != null)
            {
                explosiveObject.TakeDamage(SpellToCast.DamageAmount);
            }

            if (enemy != null)
            {
                enemy.TakeDamage(SpellToCast.DamageAmount + player.CalculateSpellDamageModifierFromRelics());

                BurnEffect burnEffect = hitCollider.gameObject.GetComponent<BurnEffect>();
                if (burnEffect == null)
                {
                    burnEffect = hitCollider.gameObject.AddComponent<BurnEffect>();
                }

                burnEffect.Initialize(SpellToCast.BurnDuration, SpellToCast.BurnParticleSystem, SpellToCast.DamagePerTick, SpellToCast.TickInterval);
            }
        }

        //int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, SpellToCast.SpellRadius * 2, spellColliders);

        //if (overlapCount > 0)
        //{
        //    for (var overlapIndex = 0; overlapIndex < overlapCount; overlapIndex++)
        //    {
        //        Enemy enemy = GetComponent<Enemy>();
        //        if (enemy != null)
        //        {
        //            enemy.TakeDamage(SpellToCast.DamageAmount);

        //            BurnEffect burnEffect = gameObject.GetComponent<BurnEffect>();
        //            if (burnEffect == null)
        //            {
        //                burnEffect = gameObject.AddComponent<BurnEffect>();
        //            }

        //            burnEffect.Initialize(SpellToCast.BurnDuration, SpellToCast.BurnParticleSystem, SpellToCast.DamagePerTick, SpellToCast.TickInterval);
        //        }
        //    }
        //}

        Destroy(this.gameObject);
    }

    private void BossDealDamageInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SpellToCast.SpellRadius * 2);

        foreach (Collider hitCollider in hitColliders)
        {
            player = hitCollider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(SpellToCast.DamageAmount);

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
