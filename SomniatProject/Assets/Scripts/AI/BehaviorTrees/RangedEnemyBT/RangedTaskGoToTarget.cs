using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
public class RangedTaskGoToTarget : Node
{
    private Transform transform;
    private Animator animator;
    private NavMeshAgent agent;

    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public RangedTaskGoToTarget(Transform transform)
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
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RangedEnemyBT.rotationSpeed * Time.deltaTime);



        if (Vector3.Distance(transform.position, target.position) > RangedEnemyBT.attackRange)
        {
            agent.SetDestination(target.position);


            float currentTime = Time.deltaTime - followStartTime;
            agent.speed = RangedEnemyBT.targetedSpeed;

            animator.SetBool("Walk", true);

            if (Vector3.Distance(transform.position, target.position) > RangedEnemyBT.distance || currentTime > maxFollowTime)
            {
                animator.SetBool("Walk", false);
                agent.speed = RangedEnemyBT.speed;
                ClearData("target");
            }
        }



        followStartTime += Time.deltaTime;




        state = NodeState.RUNNING;
        return state;
    }
}

