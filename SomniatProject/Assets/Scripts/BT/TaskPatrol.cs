using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskPatrol : Node
{
    private Transform transform;
    public Transform[] waypoints;
    private Animator animator;
    private NavMeshAgent agent;
    private int currentWaypointIndex;

    private float waitTime = 1.5f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
        this.waypoints = waypoints;

    }

    public override NodeState Evaluate()
    {

        Transform wp = waypoints[currentWaypointIndex];


        // Calculate the direction to the waypoint
        Vector3 directionToWaypoint = (wp.position - transform.position).normalized;

        // Calculate the rotation to look at the waypoint smoothly
        Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);

        // Smoothly rotate towards the waypoint
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, GuardBT.rotationSpeed * Time.deltaTime);


        if (waiting)
        {
            waitCounter += Time.deltaTime;
            animator.SetBool("Walk", false);


            if (waitCounter >= waitTime)
            {
                waiting = false;
            }
                
        }
        else
        {
            animator.SetBool("Walk", true);


            if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;
                
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {

                agent.SetDestination(wp.position);
            }
        }

        

        state = NodeState.RUNNING;
        return state;
    }

}
