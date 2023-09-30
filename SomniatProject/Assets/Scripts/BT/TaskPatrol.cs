using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform transform;
    public Transform[] waypoints;
    private Animator animator;
    private int currentWaypointIndex;

    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        this.waypoints = waypoints;

    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waiting = false;

                if(transform.position.normalized == Vector3.left)
                    animator.SetBool("Left", true);
                if(transform.position.normalized == Vector3.right)
                    animator.SetBool("Right", true);
                if (transform.position.normalized == Vector3.up)
                    animator.SetBool("Up", true);
                if (transform.position.normalized == Vector3.down)
                    animator.SetBool("Down", true);
            }
                
        }
        else
        {
            Transform wp = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;
                animator.SetBool("Idle", true);

                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {


                transform.position = Vector3.MoveTowards(transform.position, wp.position, GuardBT.speed * Time.deltaTime);
            }
        }

        

        state = NodeState.RUNNING;
        return state;
    }

}
