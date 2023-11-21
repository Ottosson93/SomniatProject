using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class Player : MonoBehaviour
{
    public float maxLucidity;
    public float lucidity;
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelligence;
    public readonly float baseSpeed = 2.0f;
    public readonly float baseMeleeDamage = 5.0f;
    public readonly float baseAttackSpeed = 1.0f;
    public float damageReduction = 1.0f;
    public float flatSpeed = 0;
    public float speed;
    public float meleeDamage;
    public float attackSpeed;

    private float originalMeleeDamage;
    private float originalAttackSpeed;
    private float originalSpeed;
    private float originalArmorAmount;

    public PlayerStats playerStats;
    public float newSpeed;

    public ThirdPersonController controller;
    public LucidityPostProcess lucidityPostProcess;

    public EmpoweredRelic empoweredRelic;

    void Start()
    {
        Strength.RemoveAllModifiersFromSource(this);
        Dexterity.RemoveAllModifiersFromSource(this);
        Intelligence.RemoveAllModifiersFromSource(this);
        controller = GetComponent<ThirdPersonController>();

        speed = baseSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;


        maxLucidity = CalculateMaxLucity();
        lucidity = maxLucidity;
        controller.MoveSpeed = CalculateSpeed();


    }

    public void SetOriginalValues()
    {
        originalAttackSpeed = attackSpeed;
        originalMeleeDamage = meleeDamage;
        originalSpeed = newSpeed;
        originalArmorAmount = damageReduction;
    }

    public void ResetAttributesToOriginal()
    {
        meleeDamage = originalMeleeDamage;
        attackSpeed = originalAttackSpeed;
        newSpeed = originalSpeed;
        damageReduction = originalArmorAmount;
    }

    public float CalculateSpeed()
    {
        return newSpeed = baseSpeed * (1 + (playerStats.Dexterity.Value / baseSpeed)) + flatSpeed;
    }

    public float CalculateAttackSpeed()
    {
        return baseAttackSpeed * (1 + (playerStats.Dexterity.Value));
    }

    float CalculateDamage()
    {
        return 1.0f;
    }
    float CalculateArmor()
    {
        return 1.0f;
    }
    float CalculateMaxLucity()
    {
        if (playerStats.Strength.Value == 0)
        {
            return 20f;
        }
        return playerStats.Strength.Value * 20;
    }

    public void IncreaseDamage(float amount)
    {
        meleeDamage *= amount;
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed *= amount;
    }

    public void IncreaseSpeed(float amount)
    {
        newSpeed *= amount;
    }

    public void ArmorReduction(float amount)
    {
        damageReduction *= amount;
    }



    public void UpdateCharacterStats()
    {
        float adjuster = lucidity / maxLucidity;
        maxLucidity = CalculateMaxLucity();
        lucidity = adjuster * maxLucidity;
        lucidityPostProcess.UpdateLucidityMask(lucidity);
        GetComponent<ThirdPersonController>().MoveSpeed = CalculateSpeed();

    }

    public void TakeDamage(float damage)
    {
        lucidity -= (damage * damageReduction);
        lucidity = Mathf.Clamp(lucidity, 0f, maxLucidity);  // Ensure lucidity is within the valid range

        lucidityPostProcess.UpdateLucidityMask(lucidity);
        
        if (lucidity <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Heal(float amountHealed)
    {
        lucidity += amountHealed;

        lucidityPostProcess.UpdateLucidityMask(lucidity);
        
        if (lucidity > maxLucidity)
        {
            lucidity = maxLucidity;
        }
    }

    public void Equip(RelicData d)
    {
        foreach (StatModifier s in d.GetModifiers())
        {
            switch (s.characterStatType)
            {
                case StatModifier.CharacterStatType.Dexterity:
                    playerStats.Dexterity.AddModifier(s);
                    break;
                case StatModifier.CharacterStatType.Strength:
                    playerStats.Strength.AddModifier(s);
                    break;
                case StatModifier.CharacterStatType.Intelligence:
                    playerStats.Intelligence.AddModifier(s);
                    break;
            }
            d.relicQuantity++;
        }
        UpdateCharacterStats();
    }

    public void Unequip(RelicData d)
    {
        playerStats.Strength.RemoveAllModifiersFromSource(d);
        playerStats.Dexterity.RemoveAllModifiersFromSource(d);
        playerStats.Intelligence.RemoveAllModifiersFromSource(d);
    }
}