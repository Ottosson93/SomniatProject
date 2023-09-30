using System.Collections.Generic;
using UnityEditor;
using BehaviorTree;

public class GuardMeleeBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2f;
    public static float targetedSpeed = 4f;
    public static float fovRange = 7f;
    public static float distance = 9f;
    public static float attackRange = 3f;
    public static float rotationSpeed = 3f;
    public static int attackDamage = 10;



    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskMeleeAttack(transform),
            }),

            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform, waypoints),
        }); 

        return root;
    }
}
