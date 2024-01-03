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

    public float lastClickedTime;
    public float lastComboEnd;
    


    public TaskMeleeAttack(Transform transform, List<AttackSO> combo, Animator animator)
    {
        this.combo = combo;
        this.animator = animator;
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

            //Audio
            AudioManager.instance.PlaySingleSFX(SoundEvents.instance.gruntAttackHit, enemy.transform.position);
            player.TakeDamage(GuardMeleeBT.attackDamage);
            
        }

        



        if(GuardMeleeBT.comboCounter >= combo.Count)
        {
            GuardMeleeBT.comboCounter = 0;
        }


        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.65f
                && animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            AudioManager.instance.PlaySingleSFX(SoundEvents.instance.gruntSwing, enemy.transform.position);
            GuardMeleeBT.comboCounter = 0;
            lastComboEnd = Time.time;
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
