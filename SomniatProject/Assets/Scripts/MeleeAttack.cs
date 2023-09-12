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
        if (!!isAttacking && attackAction.triggered)
        {
            animator.SetTrigger(currentAttackAnimation);

            currentAttackAnimation = (currentAttackAnimation == "Attack01") ? "Attack02" : "Attack01";

            comboCount++;

            if(comboCount >= maxCombo)
            {
                comboCount = 0;
            }

            isAttacking = true;
        }
    }
}
