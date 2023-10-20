using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class BossCheckRangeRushAttack : Node
{
    private Transform transform;
    private List<AttackSO> combo;
    private Animator animator;
    private NavMeshAgent agent;
    private Enemy enemy;



    public BossCheckRangeRushAttack(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
        enemy = transform.GetComponent<Enemy>();

    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if(  enemy.current < 250)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("RushAttack", true);
            agent.SetDestination(target.position);

            target.rotation = Quaternion.LookRotation(agent.velocity).normalized;

            state = NodeState.SUCCESS;
            return state;
        }



        state = NodeState.FAILURE;
        return state;

    }
}
