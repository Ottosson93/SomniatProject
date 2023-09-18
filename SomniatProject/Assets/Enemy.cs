using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    


    public void TakeDamage() 
    {
        animator.SetTrigger("Hurt");
        
    }


    void Die() { 
        animator.SetBool("Dead", true);
        GetComponent<MeshCollider>().enabled = false;
        this.enabled = false;
    }



}
