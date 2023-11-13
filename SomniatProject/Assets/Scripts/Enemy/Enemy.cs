using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public DamgeTextPlayer damageTextPlayer;
    public ItemDropSystem itemDropSystem;
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

        if (itemDropSystem == null)
        {
            Debug.LogError("ItemDropSystem component not found on the same GameObject or its parents.");
        }
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
        itemDropSystem.HandleEnemyDeath(transform.position);

        dead = true;                
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}
