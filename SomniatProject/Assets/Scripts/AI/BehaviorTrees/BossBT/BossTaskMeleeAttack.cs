using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class BossTaskMeleeAttack : Node
{
    public float bossMeleeDamage = 25;
    private Transform lastTarget;
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    private List<AttackSO> combo;
    private Enemy enemy;

    


    public BossTaskMeleeAttack(Transform transform, List<AttackSO> combo)
    {
        this.combo = combo;
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

        


        if (BossBT.comboCounter >= combo.Count)
        {
            BossBT.comboCounter = 0;
        }


        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f
                && animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            AudioManager.instance.PlaySingleSFX(SoundEvents.instance.bossMelee, enemy.transform.position);
            BossBT.comboCounter = 0;
            BossBT.lastComboEnd = Time.time;
        }


       



        if (player.lucidity <= 0f)
        {
            
            ClearData("target");
            animator.SetBool("Walk", true);
        }
        else
        {
            Collider[] hitEnemies = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                player.TakeDamage(bossMeleeDamage);
            }
    
            animator.SetBool("Walk", false);
        }

            



        state = NodeState.RUNNING;
        return state;
    }





}
