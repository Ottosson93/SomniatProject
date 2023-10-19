using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class BossTaskBoulderAttack : Node
{
    private Transform transform;
    private Transform castPos;
    private Transform lastTarget;
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    private SpellAttackSystem spell;
    private List<Spell> spells;

    public BossTaskBoulderAttack(Transform transform, List<Spell> spells, Transform castPos)
    {
        this.transform = transform;
        this.spells = spells;
        this.castPos = castPos;
        animator = transform.GetComponent<Animator>();
        spell = transform.GetComponent<SpellAttackSystem>();
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != lastTarget)
        {
            player = target.GetComponent<Player>();

            lastTarget = target;
        }



        spell.AICastSpell(spells[0], castPos.transform, player.transform);







        if (player.lucidity <= 0f)
        {
            ClearData("target");
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);

        }





        state = NodeState.RUNNING;
        return state;
    }
}
