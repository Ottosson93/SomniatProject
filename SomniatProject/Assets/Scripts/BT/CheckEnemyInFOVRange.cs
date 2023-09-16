using BehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{

    private Transform transform;
    private Animator animator;


    public CheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
        animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {


        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GuardBT.fovRange);

            foreach (Collider collider in colliders)
            {

                // Check if the collider's game object is the player
                if (collider.CompareTag("Player"))
                {
                    parent.parent.SetData("target", collider.transform);
                    animator.SetBool("Walk", true);


                    state = NodeState.SUCCESS;
                    return state;
                }
                else
                    animator.SetBool("Walk", false);

            }

            

            state = NodeState.FAILURE;
            return state;
        }

            state = NodeState.SUCCESS;
        return state;
    }
}
