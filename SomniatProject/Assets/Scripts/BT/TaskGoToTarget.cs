using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using UnityEngine.AI;

public class TaskGoToTarget : Node
{
    private Transform transform;
    private NavMeshAgent agent;
    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public TaskGoToTarget(Transform transform)
    {
        this.transform = transform;
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(Vector2.Distance(transform.position, target.position) > 0.01f)
        {


            // Calculate the direction to the waypoint
            Vector3 directionToWaypoint = (target.position - transform.position).normalized;

            // Calculate the rotation to look at the waypoint smoothly
            Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);

            // Smoothly rotate towards the waypoint
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GuardBT.rotationSpeed * Time.deltaTime);

            agent.SetDestination(target.position);

            float currentTime = Time.deltaTime - followStartTime;

            if (Vector2.Distance(transform.position, target.position) > GuardBT.distance || currentTime > maxFollowTime)
                ClearData("target");
        }

        followStartTime += Time.deltaTime;

        
        

        state = NodeState.RUNNING;
        return state;
    }
}
