using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public DamgeTextPlayer damageTextPlayer;


    public int health;
    public int current;

    public bool dead = false;

    private void Start()
    {
        current = health;
    }


    public void TakeDamage() 
    {
        current -= 10;

        animator.SetTrigger("Hurt");
        damageTextPlayer.SubtractHealth(10, transform);

        

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
    }



}
