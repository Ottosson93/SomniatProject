using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
public class CheckEnemyInRangedAttack : Node
{
    private Transform transform;
    private Animator animator;
    private NavMeshAgent agent;



    public CheckEnemyInRangedAttack(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();

    }

    public override NodeState Evaluate()
    {

        

        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 12f);


            
            foreach (Collider collider in colliders)
            {

                // Check if the collider's game object is the player
                if (collider.CompareTag("Player"))
                {
                    parent.parent.SetData("target", collider.transform);

                    state = NodeState.SUCCESS;
                    return state;
                }


            }


            

            
        }
        state = NodeState.FAILURE;
        return state;

    }
}
