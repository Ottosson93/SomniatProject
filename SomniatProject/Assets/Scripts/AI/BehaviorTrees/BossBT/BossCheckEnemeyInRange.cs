using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class BossCheckEnemyInAttackRange : Node
{
    private Transform transform;
    private List<AttackSO> combo;
    private Animator animator;
    private NavMeshAgent agent;



    public BossCheckEnemyInAttackRange(Transform transform, List<AttackSO> combo)
    {
        this.transform = transform;
        this.combo = combo;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();

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

        if (Vector3.Distance(transform.position, target.position) <= BossBT.attackRange)
        {
            if (Time.time - BossBT.lastClickedTime > 2.5f && BossBT.comboCounter <= combo.Count)
            {
                if (Time.time - BossBT.lastClickedTime >= 2f)
                {
                    animator.runtimeAnimatorController = combo[BossBT.comboCounter].animatorOV;
                    animator.Play("Attack", 1, 0);
                    BossBT.attackDamage = combo[BossBT.comboCounter].damage;

                    BossBT.comboCounter = BossBT.comboCounter + 1;
                    BossBT.lastClickedTime = Time.time;

                    agent.speed = 0f;

                    state = NodeState.SUCCESS;
                    return state;



                }

            }


        }

        BossBT.comboCounter = 0;

        state = NodeState.FAILURE;
        return state;

    }
}
