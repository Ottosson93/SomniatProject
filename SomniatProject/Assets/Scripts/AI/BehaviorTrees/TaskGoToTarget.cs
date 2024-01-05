using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
public class TaskGoToTarget : Node
{
    private Transform transform;
    private Animator animator;
    private Animator lucidAnimator;
    private NavMeshAgent agent;

    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public TaskGoToTarget(Transform transform, Animator animator)
    {
        this.transform = transform;
        this.animator = animator;
        agent = transform.GetComponent<NavMeshAgent>();
        lucidAnimator = transform.Find("LucidMesh").GetComponent<Animator>();
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
            //animator.SetBool("Walk", true);
            //lucidAnimator.SetBool("Walk", true);

            agent.SetDestination(target.position);
            float currentTime = Time.deltaTime - followStartTime;
            agent.speed = GuardMeleeBT.targetedSpeed;

         

            if (Vector3.Distance(transform.position, target.position) > GuardMeleeBT.distance || currentTime > maxFollowTime)
            {
                animator.SetBool("Walk", false);
                lucidAnimator.SetBool("Walk", false);
                //agent.speed = GuardMeleeBT.speed;
                ClearData("target");
            }
        }



        followStartTime += Time.deltaTime;

        
        

        state = NodeState.RUNNING;
        return state;
    }
}
