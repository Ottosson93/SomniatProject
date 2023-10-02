using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskMeleeAttack : Node
{

    private Transform transform;
    private NavMeshAgent agent;


    public TaskMeleeAttack(Transform transform)
    {
        this.transform = transform;
        agent = transform.GetComponent<NavMeshAgent>();
    }


    public override NodeState Evaluate()
    {
        object target = (Transform)GetData("target");
        agent.speed = 0f;


        state = NodeState.RUNNING;
        return state;
    }


   

    
}
