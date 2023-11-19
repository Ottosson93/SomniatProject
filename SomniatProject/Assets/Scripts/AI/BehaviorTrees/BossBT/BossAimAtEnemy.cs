using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class BossAimAtEnemy : Node
{
    private Transform transform;
    private Animator animator;
    private NavMeshAgent agent;

    private float maxFollowTime = 3f;   // Adjust as needed
    private float followStartTime = 0f;



    public BossAimAtEnemy(Transform transform)
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
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, BossBT.rotationSpeed * Time.deltaTime);



        if (Vector3.Distance(transform.position, target.position) > 20f)
        {
            float currentTime = Time.deltaTime - followStartTime;



            if (Vector3.Distance(transform.position, target.position) > 20f || currentTime > maxFollowTime)
            {
                animator.SetBool("Walk", false);
                agent.speed = BossBT.speed;
                ClearData("target");
            }
        }



        followStartTime += Time.deltaTime;

        if(BossBT.bossSpellCounter >= 3)
        {
            state = NodeState.FAILURE;
            return state;
        } 


        state = NodeState.RUNNING;
        return state;
    }
}
