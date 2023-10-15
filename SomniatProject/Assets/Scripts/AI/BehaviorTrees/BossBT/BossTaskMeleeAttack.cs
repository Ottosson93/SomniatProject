using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class BossTaskMeleeAttack : Node
{

    private Transform lastTarget;
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    private List<AttackSO> combo;


    public BossTaskMeleeAttack(Transform transform, List<AttackSO> combo)
    {
        this.combo = combo;
        animator = transform.GetComponent<Animator>();

    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != lastTarget)
        {
            player = target.GetComponent<Player>();

            lastTarget = target;
        }


        

        if (BossBT.comboCounter >= combo.Count)
        {
            BossBT.comboCounter = 0;
        }


        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.99f
                && animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            player.TakeDamage(BossBT.attackDamage);

            BossBT.comboCounter = 0;
            BossBT.lastComboEnd = Time.time;
        }



        if (player.lucidity <= 0f)
        {
            ClearData("target");
            animator.SetBool("Walk", true);
        }
        else
            BossBT.attackCounter = 0;



        state = NodeState.RUNNING;
        return state;
    }





}
