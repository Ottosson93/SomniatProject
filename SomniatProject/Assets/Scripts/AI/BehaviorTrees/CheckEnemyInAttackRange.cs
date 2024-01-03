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
    private Animator lucidAnimator;
    private NavMeshAgent agent;

    float lastClickedTime;
    float lastComboEnd;
    

    public CheckEnemyInAttackRange(Transform transform, List<AttackSO> combo, Animator animator)
    {
        this.transform = transform;
        this.combo = combo;
        this.animator = animator;
        agent = transform.GetComponent<NavMeshAgent>();
        lucidAnimator = transform.Find("LucidMesh").GetComponent<Animator>();

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

            if(GuardMeleeBT.comboCounter <= combo.Count)
            {
                if (Time.time - lastClickedTime > GuardMeleeBT.attackRate)
                {
                    

                    if (Time.time - lastClickedTime >= GuardMeleeBT.attackRate)
                    {
                        animator.runtimeAnimatorController = combo[GuardMeleeBT.comboCounter].animatorOV;
                        animator.Play("Attack", 1, 0.17f);

                        GuardMeleeBT.attackDamage = combo[GuardMeleeBT.comboCounter].damage;



                        GuardMeleeBT.comboCounter = GuardMeleeBT.comboCounter + 1;
                        lastClickedTime = Time.time;

                        agent.speed = 0f;
                        
                        animator.SetBool("Run", false);
                        lucidAnimator.SetBool("Run", false);
                        state = NodeState.SUCCESS;
                        return state;

                    }

                }
            }

            

           
        }

        

        state = NodeState.FAILURE;
        return state;

    }
}
