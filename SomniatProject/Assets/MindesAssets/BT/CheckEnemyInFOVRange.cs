using BehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{

    private Transform transform;


    public CheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {

        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GuardMeleeBT.fovRange);

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

            

            state = NodeState.FAILURE;
            return state;
        }

            state = NodeState.SUCCESS;
        return state;
    }
}
