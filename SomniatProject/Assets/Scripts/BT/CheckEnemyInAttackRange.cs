using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform transform;

    public CheckEnemyInAttackRange(Transform transform)
    {
        this.transform = transform;
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

        if(Vector2.Distance(transform.position, target.position) <= GuardBT.attackRange)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }
}
