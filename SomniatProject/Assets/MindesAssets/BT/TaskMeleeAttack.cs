using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskAttack : Node
{

    private Transform transform;
    private EnemyShooting enemyShooting;
    private NavMeshAgent agent;


    public TaskAttack(Transform transform, EnemyShooting enemyShooting)
    {
        this.transform = transform;
        this.enemyShooting = enemyShooting;
        agent = transform.GetComponent<NavMeshAgent>();
    }


    public override NodeState Evaluate()
    {
        object target = (Transform)GetData("target");
        //agent.speed = 0f;
        enemyShooting.timer += Time.deltaTime;

        if (enemyShooting.timer > enemyShooting.cooldownTime)
        {
            enemyShooting.timer = 0;
            enemyShooting.Shoot();
        }

        state = NodeState.RUNNING;
        return state;
    }





}
