using System.Collections.Generic;
using UnityEditor;
using BehaviorTree;

public class RangedEnemyBT : Tree
{
    public List<AttackSO> combo;
    public RangedEnemyShoot enemyShoot;

    public static float speed = 2f;
    public static float targetedSpeed = 4f;
    public static float fovRange = 11f;
    public static float distance = 12f;
    public static float attackRange = 10f;
    public static float rotationSpeed = 3f;
    public static float attackDamage = 2f;
    public static bool canAttack = true;

    public static float lastClickedTime;
    public static float lastComboEnd;
    public static int comboCounter;

    public static float attackRate = 0.4f;
    public static int attackCounter = 0;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new RangedCheckEnemyInRange(transform, combo),
                new RangedTaskRangedAttack(transform, enemyShoot), /* Ranged task attack goes here*/
            }),

            new Sequence(new List<Node>
            {
                new RangedCheckEnemyInFOVRange(transform),
                new RangedTaskGoToTarget(transform),
            }),
        });

        return root;
    }
}
