using System.Collections.Generic;
using UnityEditor;
using BehaviorTree;

public class GuardMeleeBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public List<AttackSO> combo;
    

    public static float speed = 2f;
    public static float targetedSpeed = 4f;
    public static float fovRange = 7f;
    public static float distance = 9f;
    public static float attackRange = 2.5f;
    public static float rotationSpeed = 3f;
    public static float attackDamage = 10f;
    public static bool canAttack = true;

    public static float lastClickedTime;
    public static float lastComboEnd;
    public static int comboCounter;

    public static float attackRate = 2f;
    public static int attackCounter = 0;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform, combo),
                new TaskMeleeAttack(transform, combo),
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
