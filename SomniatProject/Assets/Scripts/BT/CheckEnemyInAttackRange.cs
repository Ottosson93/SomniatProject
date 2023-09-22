using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform transform;
    private Animator animator;
    private NavMeshAgent agent;
    public CheckEnemyInAttackRange(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if(t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if(Vector3.Distance(transform.position, target.position) <= GuardMeleeBT.attackRange)
        {
            agent.speed = 0f;
            animator.SetTrigger("Attack");
            animator.SetBool("Run", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }
}
