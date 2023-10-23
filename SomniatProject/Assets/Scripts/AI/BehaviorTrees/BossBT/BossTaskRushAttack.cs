using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class BossTaskRushAttack : Node
{
    private Transform transform;
    private Transform lastTarget;
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    private List<AttackSO> combo;
    private Enemy enemy;


    public BossTaskRushAttack(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        enemy = transform.GetComponent<Enemy>();
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != lastTarget)
        {
            player = target.GetComponent<Player>();

            lastTarget = target;
        }




        if (player.lucidity <= 0f)
        {
            ClearData("target");
            animator.SetBool("Walk", true);
        }
        else
        {
            //Collider[] hitEnemies = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.enemyLayer);

            //foreach (Collider enemy in hitEnemies)
            //{
            //    player.TakeDamage(BossBT.rushDamage);
            //}


            

            animator.SetBool("Walk", false);
            BossBT.attackCounter = 0;



            if(Time.time > 1f)
            {
                animator.SetBool("RushAttack", false);
            }
        }





        state = NodeState.RUNNING;
        return state;
    }
}
