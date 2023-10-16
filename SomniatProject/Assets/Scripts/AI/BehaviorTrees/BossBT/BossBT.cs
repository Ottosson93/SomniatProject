using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
public class BossBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Transform castPos;
    public List<AttackSO> combo;
    public List<Spell> spells;

    public static float speed = 1f;
    public static float targetedSpeed = 4f;
    public static float fovRange = 10f;
    public static float distance = 9f;
    public static float attackRange = 4f;
    public static float rotationSpeed = 3f;
    public static float attackDamage = 0;
    public static bool canAttack = true;

    public static float lastClickedTime = 0;
    public static float lastComboEnd = 0;
    public static int comboCounter;

    public static float attackRate = 2f;
    public static float attackCounter = 0f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new BossCheckEnemyInAttackRange(transform, combo),
                new BossTaskMeleeAttack(transform, combo),
                //new BossTaskBoulderAttack(transform, spells, castPos),
            }),

            new Sequence(new List<Node>
            {
                new BossCheckEnemyInFOVRange(transform),
                new BossTaskGoToTarget(transform),
            }),
            //new TaskPatrol(transform, waypoints),
        });

        return root;
    }
}
