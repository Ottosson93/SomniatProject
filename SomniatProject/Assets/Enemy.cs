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
        animator.SetTrigger("Hurt");
        damageTextPlayer.SubtractHealth(10, transform);

        current -= 10;

        if(current <= 0)
        {
            StartCoroutine(Die());
        }
        
    }


    IEnumerator Die() { 
        animator.SetBool("Die", true);
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(2f);

        dead = true;                
        Destroy(gameObject);
    }



}
