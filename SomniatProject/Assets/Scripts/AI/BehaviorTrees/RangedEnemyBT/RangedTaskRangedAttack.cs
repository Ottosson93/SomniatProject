using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;


public class RangedTaskRangedAttack : Node
{
    public Transform transform;
    public RangedEnemyShoot enemyShooting;
    public NavMeshAgent agent;
    public Vector3 direction;

    public RangedTaskRangedAttack(Transform transform, RangedEnemyShoot enemyShooting)
    {
        this.transform = transform;
        this.enemyShooting = enemyShooting;
        agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();

    }


    public override NodeState Evaluate()
    {
        //GameObject target = (Transform)GetData("target");
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        direction = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RangedEnemyBT.rotationSpeed * Time.deltaTime);
        enemyShooting.timer += Time.deltaTime;

        if (enemyShooting.timer > enemyShooting.cooldownTime)
        {
            enemyShooting.timer = 0;
            enemyShooting.Shoot();
        }

        state = NodeState.RUNNING;
        return state;
    }

}
