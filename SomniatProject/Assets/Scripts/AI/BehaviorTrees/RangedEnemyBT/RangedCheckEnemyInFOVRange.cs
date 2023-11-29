using BehaviorTree;
using UnityEngine;

public class RangedCheckEnemyInFOVRange : Node
{

    private Transform transform;


    public RangedCheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {

        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, RangedEnemyBT.fovRange);

            foreach (Collider collider in colliders)
            {

                // Check if the collider's game object is the player
                if (collider.CompareTag("Player"))
                {
                    parent.parent.SetData("target", collider.transform);
                    AudioManager.instance.AddEnemyEngage();

                    state = NodeState.SUCCESS;
                    tempStates = NodeState.SUCCESS;
                    return state;
                }


            }



            state = NodeState.FAILURE;
            if (state == NodeState.FAILURE && tempStates == NodeState.SUCCESS)
            {
                tempStates = NodeState.FAILURE;
                AudioManager.instance.removeEnemyEngage();
            }
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
