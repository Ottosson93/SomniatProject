using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskMeleeAttack : Node
{

    private Transform lastTarget;
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    private Animator lucidAnimator;
    private List<AttackSO> combo;
    private Enemy enemy;


    public TaskMeleeAttack(Transform transform, List<AttackSO> combo)
    {
        this.combo = combo;
        animator = transform.GetComponent<Animator>();
        lucidAnimator = transform.Find("LucidMesh").GetComponent<Animator>();
        enemy = transform.GetComponent<Enemy>();

    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if(target != lastTarget)
        {
            player = target.GetComponent<Player>();

            lastTarget = target;
        }


        Collider[] hitEnemies = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            player.TakeDamage(GuardMeleeBT.attackDamage);
        }

        



        if(GuardMeleeBT.comboCounter >= combo.Count)
        {
            GuardMeleeBT.comboCounter = 0;
        }


        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.99f
                && animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            GuardMeleeBT.comboCounter = 0;
            GuardMeleeBT.lastComboEnd = Time.time;
        }



        if (player.lucidity <= 0f)
        {
            ClearData("target");
            animator.SetBool("Walk", true);
            lucidAnimator.SetBool("Walk", true);
        }
        else
            GuardMeleeBT.attackCounter = 0;
        


        state = NodeState.RUNNING;
        return state;
    }


   

    
}
