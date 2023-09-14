using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    InputAction attackAction;
    public int maxCombo = 3;

    private int comboCount = 0;
    private bool isAttacking = false;
    private string currentAttackAnimation = "Attack01";

    private void Awake()
    {
        attackAction = new InputAction("Attack", binding: "<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void Update()
    {
        if (comboCount >= 3)
        {
            comboCount = 0;
        }

        if (attackAction.triggered)
        {
            comboCount++;

            if (comboCount == 1)
            {
                animator.SetTrigger("Attack01");
            }
            else if (comboCount == 2)
            {
                animator.SetTrigger("Attack02");
            }
            else if (comboCount == 3)
            {
                //Deal extra dmg
                comboCount = 1;
                animator.SetTrigger("Attack01");
            }
        }

    }
}
