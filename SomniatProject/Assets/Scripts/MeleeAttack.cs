using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public int maxCombo = 3;
    public Sword sword;


    private int comboCount = 0;
    private InputAction attackAction;

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
        if (comboCount >= maxCombo)
        {
            comboCount = 0;
        }

        if (attackAction.triggered)
        {
            Debug.Log("Attack sequence triggered");
            comboCount++;

            if (comboCount == 1)
            {
                Debug.Log("Attack 1 triggered");
                animator.SetTrigger("Attack01");
                sword.Attack();
            }
            else if (comboCount == 2)
            {
                Debug.Log("Attack 2 triggered");
                animator.SetTrigger("Attack02");
                sword.Attack();
            }
            else if (comboCount == 3)
            {
                Debug.Log("Attack 3 triggered");
                comboCount = 0;
                animator.SetTrigger("Attack01");
                sword.Attack();
            }
        }
    }
}
