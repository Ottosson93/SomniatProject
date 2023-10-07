using System.Collections.Generic;
using UnityEditor;
using BehaviorTree;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.GameObject bulletPrefab;
    public UnityEngine.Transform bulletSpawnPoint;
    public EnemyShooting enemyShooting;
    List<AttackSO> combo;
    
    
    public static float speed = 2f;
    public static float fovRange = 7f;
    public static float distance = 9f;
    public static float attackRange = 5f;
    public static float bulletForce = 20f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform, combo),
                new TaskAttack(transform, enemyShooting),
            }),

            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            //new TaskPatrol(transform, waypoints),
        }); 

        return root;
    }
}
