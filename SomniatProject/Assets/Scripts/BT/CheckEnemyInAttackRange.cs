using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform transform;
    private Animator animator;

    public CheckEnemyInAttackRange(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
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

        if(Vector2.Distance(transform.position, target.position) <= GuardBT.attackRange)
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Walk", false);

            state = NodeState.SUCCESS;
            return state;
        }

        animator.SetBool("Attack", false);
        animator.SetBool("Walk", true);

        state = NodeState.FAILURE;
        return state;

    }
}
