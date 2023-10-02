using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public Animator animator;
    public DamgeTextPlayer damageTextPlayer;



    public void TakeDamage() 
    {
        animator.SetTrigger("Hurt");
        damageTextPlayer.SubtractHealth(10, transform);


    }


    void Die() { 
        animator.SetBool("Dead", true);
        GetComponent<MeshCollider>().enabled = false;
        this.enabled = false;
    }



}
