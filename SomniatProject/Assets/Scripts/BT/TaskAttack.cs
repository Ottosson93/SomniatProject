using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskMeleeAttack : Node
{

    private Transform transform;
    


    public TaskMeleeAttack(Transform transform)
    {
        this.transform = transform;

    }


    public override NodeState Evaluate()
    {
        object target = (Transform)GetData("target");


        state = NodeState.RUNNING;
        return state;
    }


   

    
}
