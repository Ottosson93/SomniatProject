using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ItemDropSystem itemDropSystem;
    public Animator animator;
    public DamgeTextPlayer damageTextPlayer;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public int health = 100;
    public int current;
    public float attackRange;


    public bool dead = false;

    private void Start()
    {
        current = health;

        itemDropSystem = GetComponent<ItemDropSystem>();
    }


    public void TakeDamage(int damage) 
    {
        current = current - damage;
        animator.SetTrigger("Hurt");
        damageTextPlayer.SubtractHealth(damage, transform);

        if(current <= 0)
        {
            Die();
        }
    }

   

    void Die() {

        animator.SetBool("Die", true);
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;

        dead = true;                
        Destroy(gameObject);

        if (itemDropSystem != null)
        {
            itemDropSystem.HandleEnemyDeath(transform.position);
        }
    }


    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}
