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
        agent = transform.GetComponent<NavMeshAgent>();
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != lastTarget)
        {
            player = target.GetComponent<Player>();

            lastTarget = target;
        }

       
            

        if (Time.time - BossBT.lastClickedTime > 2.5f )
        {
            if (Time.time - BossBT.lastClickedTime >= 2f)
            {

                animator.SetBool("Walk", false);

                BossBT.comboCounter = BossBT.comboCounter + 1;
                BossBT.lastClickedTime = Time.time;

                spell.AICastSpell(spells[0], castPos.transform, player.transform);


                agent.speed = 0f;


            }

        }



        animator.SetBool("Walk", true);





        if (player.lucidity <= 0f)
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }
        





        state = NodeState.RUNNING;
        return state;
    }
}
