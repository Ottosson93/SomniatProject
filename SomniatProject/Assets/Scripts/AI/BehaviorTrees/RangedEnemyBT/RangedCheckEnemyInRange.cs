using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class RangedCheckEnemyInRange : Node
{
    private Transform transform;
    private List<AttackSO> combo;
    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 realTarget;
    private float yTargetOffset = 5f;

    public RangedCheckEnemyInRange(Transform transform, List<AttackSO> combo)
    {
        this.transform = transform;
        this.combo = combo;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();

    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        //if (t == null)
        //{
        //    state = NodeState.FAILURE;
        //    return state;
        //}

        //Transform target = (Transform)t;
        Transform target = GameObject.FindGameObjectWithTag("Player").transform; 
        realTarget = new Vector3(target.position.x, target.position.y + yTargetOffset);

        if (Vector3.Distance(transform.position, target.position) <= RangedEnemyBT.attackRange)
        {
            //Debug.Log("Ranged enemy range check");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }



    //public override NodeState Evaluate()
    //{
    //    object t = GetData("target");

    //    if (t == null)
    //    {
    //        state = NodeState.FAILURE;
    //        return state;
    //    }

    //    Transform target = (Transform)t;

    //    if (Vector3.Distance(transform.position, target.position) <= RangedEnemyBT.attackRange /*&& RangedEnemyBT.canAttack*/)
    //    {
    //        if (Time.time - RangedEnemyBT.lastClickedTime > 2.5f && RangedEnemyBT.comboCounter <= combo.Count)
    //        {
    //            if (Time.time - RangedEnemyBT.lastClickedTime >= 2f)
    //            {
    //                animator.runtimeAnimatorController = combo[RangedEnemyBT.comboCounter].animatorOV;
    //                animator.Play("Attack", 1, 0);
    //                RangedEnemyBT.attackDamage = combo[RangedEnemyBT.comboCounter].damage;

    //                RangedEnemyBT.comboCounter = RangedEnemyBT.comboCounter + 1;
    //                RangedEnemyBT.lastClickedTime = Time.time;

    //                agent.speed = 0f;

    //                state = NodeState.SUCCESS;
    //                return state;



    //            }

    //        }


    //    }

    //    state = NodeState.FAILURE;
    //    return state;

    //}
}
