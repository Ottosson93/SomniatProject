using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    public float force;
    private float yDirectionOffset = 0.7f;
    public int damage = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;

        rb.velocity = new Vector3(direction.x, direction.y + yDirectionOffset, direction.z).normalized * force;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            AudioManager.instance.PlaySingleSFX(SoundEvents.instance.rangedAttackHit, other.transform.position);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
        Destroy(gameObject);
    }
}
