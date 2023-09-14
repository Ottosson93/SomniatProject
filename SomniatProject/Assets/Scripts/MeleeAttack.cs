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

    // Start is called before the first frame update
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

    // Update is called once per frame
    private void Update()
    {
        if (comboCount >= 3)
        {
            comboCount = 0;
        }

        if (Input.GetButtonDown("AttackButton")) // Replace "AttackButton" with your actual attack input.
        {
            comboCount++;

            if (comboCount == 1)
            {
                animator.SetTrigger("Attack01"); // First press, trigger "Attack01."
            }
            else if (comboCount == 2)
            {
                animator.SetTrigger("Attack02"); // Second press, trigger "Attack02."
            }
            else if (comboCount == 3)
            {
                comboCount = 1; // Third press, reset comboCount to 1.
                animator.SetTrigger("Attack01"); // Set to trigger "Attack01" again.
            }
        }

    }
}
