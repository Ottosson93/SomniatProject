using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskGoToTarget : Node
{
    private Transform transform;

    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public TaskGoToTarget(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, GuardBT.speed * Time.deltaTime);

            float currentTime = Time.deltaTime - followStartTime;

            if (Vector2.Distance(transform.position, target.position) > GuardBT.distance || currentTime > maxFollowTime)
                ClearData("target");
        }

        followStartTime += Time.deltaTime;

        
        

        state = NodeState.RUNNING;
        return state;
    }
}
