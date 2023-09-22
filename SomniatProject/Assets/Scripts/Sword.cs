using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Sword : MonoBehaviour
{

    public int damage;
    public float attackCooldown;
    public float attackRange;

    private bool canAttack;
    
    public void Attack()
    {
        if(canAttack)
        {
            Debug.Log("Sword attack! Damage: " + damage);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach(var  hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    //hitCollider.GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            canAttack = false;
            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


