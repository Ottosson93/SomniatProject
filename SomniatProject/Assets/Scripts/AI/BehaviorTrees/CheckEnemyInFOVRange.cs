using BehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{

    private Transform transform;
    private Enemy enemy;
    private Animator animator;
    private Animator lucidAnimator;

    public CheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
        enemy = transform.GetComponent<Enemy>();
        animator = transform.GetComponent<Animator>();
        lucidAnimator = transform.Find("LucidMesh").GetComponent<Animator>();
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

                    animator.SetBool("Walk", true);
                    lucidAnimator.SetBool("Walk", true);

                    AudioManager.instance.AddEnemyEngage();
                    enemy.engaged = true;
                    state = NodeState.SUCCESS;
                    tempStates = NodeState.SUCCESS;
                    return state;
                }
               
                
                
            }

            

            state = NodeState.FAILURE;
            if(state == NodeState.FAILURE && tempStates == NodeState.SUCCESS)
            {
                tempStates = NodeState.FAILURE;
                enemy.engaged = false;
                AudioManager.instance.removeEnemyEngage();
            }
            return state;
        }

            state = NodeState.SUCCESS;
        return state;
    }
}
