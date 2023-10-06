using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public DamgeTextPlayer damageTextPlayer;

    public bool isBeingHit = false;
    public int health = 100;
    public int current;

    public bool dead = false;

    private void Start()
    {
        current = health;
    }


    public void TakeDamage(int damage) 
    {
        isBeingHit = true;
        StartCoroutine(ResetHitFlagAfterDelay(1f));
        current -= damage;
        animator.SetTrigger("Hurt");
        damageTextPlayer.SubtractHealth(damage, transform);

        if(current <= 0)
        {
            Die();
        }
    }

    public IEnumerator ResetHitFlagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBeingHit = false;
    }


    void Die() {

        animator.SetBool("Die", true);
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;

        dead = true;                
        Destroy(gameObject);
    }



}
