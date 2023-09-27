using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskMeleeAttack : Node
{

    private Transform lastTarget;
    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    private Animator animator;


    private float attackTime = 1f;
    private float attackCounter = 0f;

    public TaskMeleeAttack(Transform transform)
    {
        animator = transform.GetComponent<Animator>();
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if(target != lastTarget)
        {
            playerHealth = target.GetComponent<PlayerHealth>();

            lastTarget = target;
        }
        attackCounter += Time.deltaTime;
        if(attackCounter >= attackTime)
        {
            playerHealth.TakeDamage(GuardMeleeBT.attackDamage);

            if(playerHealth.health <= 0f)
            {
                ClearData("target");
                animator.SetBool("Walk", true);
            }
            else
                attackCounter = 0f;
        }


        state = NodeState.RUNNING;
        return state;
    }


   

    
}
