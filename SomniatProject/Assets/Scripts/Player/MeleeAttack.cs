using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public int maxCombo = 3;
    public Sword sword;
    public float comboResetTime = 3.0f;

    public string[] attackAnimations; 

    private int comboCount = 0;
    private InputAction attackAction;
    private float comboTimer;
    public ParticleSystem swordSlashParticles;

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
        comboTimer += Time.deltaTime;

        if (comboCount >= maxCombo || comboTimer >= comboResetTime)
        {
            comboCount = 0;
            comboTimer = 0f;
        }

        if (attackAction.triggered)
        {

            Debug.Log("Attack sequence triggered");
            comboCount++;

            int animationIndex = CalculateAnimationIndex();

            if (animationIndex < attackAnimations.Length)
            {
                Debug.Log("Attack " + comboCount + " triggered");
                animator.SetTrigger(attackAnimations[animationIndex]);
                sword.Attack();
            }
            else
            {
                Debug.LogWarning("No attack animation defined for combo count: " + comboCount);
            }

            comboTimer = 0.0f;

            ActivateSwordSlashParticles();
        }
        else
        {
            DeactivateSwordSlashParticles();
        }


    }

    private void DeactivateSwordSlashParticles()
    {
        if (swordSlashParticles != null)
        {
            swordSlashParticles.Stop();
        }
    }

    private void ActivateSwordSlashParticles()
    {
        if (swordSlashParticles != null)
        {
            swordSlashParticles.Play();
        }
    }

    private int CalculateAnimationIndex()
    {
        if (comboCount == 3)
            return 0;
        else
            return comboCount - 1;
    }
}
