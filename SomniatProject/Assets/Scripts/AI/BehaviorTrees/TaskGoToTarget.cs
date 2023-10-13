using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
public class TaskGoToTarget : Node
{
    private Transform transform;
    private Animator animator;
    private NavMeshAgent agent;

    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public TaskGoToTarget(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        // Calculate the direction to the waypoint
        Vector3 directionToWaypoint = (target.position - transform.position).normalized;

        // Calculate the rotation to look at the waypoint smoothly
        Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);

        // Smoothly rotate towards the waypoint
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GuardMeleeBT.rotationSpeed * Time.deltaTime);



        if (Vector3.Distance(transform.position, target.position) > GuardMeleeBT.attackRange )
        {
            agent.SetDestination(target.position);
            

            float currentTime = Time.deltaTime - followStartTime;
            agent.speed = GuardMeleeBT.targetedSpeed;

            animator.SetBool("Run", true);

            if (Vector3.Distance(transform.position, target.position) > GuardMeleeBT.distance || currentTime > maxFollowTime)
            {
                animator.SetBool("Run", false);
                agent.speed = GuardMeleeBT.speed;
                ClearData("target");
            }
        }



        followStartTime += Time.deltaTime;

        
        

        state = NodeState.RUNNING;
        return state;
    }
}
