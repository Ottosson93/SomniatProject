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

    private List<GameObject> triggeredGameObjects = new List<GameObject>();

    [SerializeField] private ParticleSystem burnParticleSystem;


    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        Destroy(this.gameObject, SpellToCast.Lifetime);
    }


    void Start()
    {
        if (burnParticleSystem != null)
        {
            Debug.Log("Particle System is assigned.The prefab used: " + burnParticleSystem.name + "Position: " + burnParticleSystem.transform.position);
        }
        else
        {
            Debug.LogWarning("Particle System is not assigned!");
        }
    }




    private void Update()
    {
        if (SpellToCast.Speed > 0)
        {
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggeredObject = other.gameObject;
        triggeredGameObjects.Add(triggeredObject);

        foreach(GameObject gameObject in triggeredGameObjects)
        {
            Debug.Log(gameObject);
        }
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon"))
        {
            Physics.IgnoreCollision(myCollider, other);
        }
        else
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
