using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform transform;
    private List<AttackSO> combo;
    private Animator animator;
    private NavMeshAgent agent;

    

    public CheckEnemyInAttackRange(Transform transform, List<AttackSO> combo)
    {
        this.transform = transform;
        this.combo = combo;
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

        if(Vector3.Distance(transform.position, target.position) <= GuardMeleeBT.attackRange &&  GuardMeleeBT.canAttack)
        {
            if (Time.time - GuardMeleeBT.lastClickedTime > 2f && GuardMeleeBT.comboCounter <= combo.Count)
            {   
                if(Time.time - GuardMeleeBT.lastClickedTime >= 2f)
                {
                    animator.runtimeAnimatorController = combo[GuardMeleeBT.comboCounter].animatorOV;
                    animator.Play("Attack", 1, 0);
                    GuardMeleeBT.attackDamage = combo[GuardMeleeBT.comboCounter].damage;

                    GuardMeleeBT.comboCounter = GuardMeleeBT.comboCounter + 1;
                    GuardMeleeBT.lastClickedTime = Time.time;

                    agent.speed = 0f;
                    animator.SetBool("Run", false);

                    state = NodeState.SUCCESS;
                    return state;

                   
                    
                }

            }

           
        }

        

        state = NodeState.FAILURE;
        return state;

    }
}
