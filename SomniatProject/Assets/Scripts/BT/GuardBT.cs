using System.Collections.Generic;
using UnityEditor;
using BehaviorTree;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.GameObject bulletPrefab;
    public UnityEngine.Transform bulletSpawnPoint;
    public EnemyShooting enemyShooting;

    
    
    public static float speed = 2f;
    public static float fovRange = 7f;
    public static float distance = 9f;
    public static float attackRange = 1f;
    public static float bulletForce = 20f;
    public static float rotationSpeed = 5f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
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
