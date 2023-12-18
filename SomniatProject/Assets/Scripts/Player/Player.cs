using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System;

public class Player : MonoBehaviour
{
    public float maxLucidity;
    public float originalMaxLucidity = 100f;
    public float lucidity;
    public readonly float baseSpeed = 8f;
    public readonly float baseMeleeDamage = 10f;
    public readonly float baseAttackSpeed = 0.4f;
    public readonly float baseRangedAttackSpeed = 1.0f;
    public readonly float baseArmor = 1.0f;
    public float damageReduction;
    public float flatSpeed = 0;
    public float speed;
    public float meleeDamage;
    public float attackSpeed;
    public float rangedAttackSpeed;
    public static bool isDead = false;


    public float temporaryAttackSpeedModifier;
    public float temporaryMeleeDamageModifier;
    public float temporarySpeedModifier;
    public float temporaryArmorReductionModifier;
    float  armor;

    public PlayerStats playerStats;
    public float newSpeed;

    public ThirdPersonController controller;
    public LucidityPostProcess lucidityPostProcess;

    public EmpoweredRelic empoweredRelic;

    void Start()
    {
        Time.timeScale = 1f;
        controller = GetComponent<ThirdPersonController>();
        lucidityPostProcess = GetComponent<LucidityPostProcess>();

        speed = baseSpeed;
        rangedAttackSpeed = baseRangedAttackSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;
        lucidity = originalMaxLucidity;
        maxLucidity = originalMaxLucidity;
        damageReduction = baseArmor;

        controller.MoveSpeed = speed;

        temporaryArmorReductionModifier = 1.0f;
        temporaryAttackSpeedModifier = 1.0f;
        temporaryMeleeDamageModifier = 1.0f;
        temporarySpeedModifier = 1.0f;
    }

    public void StartBerserk(float armorReduction, float attackSpeedBoost, float damageBoost, float movementSpeedBoost)
    {
        temporaryArmorReductionModifier = armorReduction;
        temporaryAttackSpeedModifier = attackSpeedBoost;
        temporaryMeleeDamageModifier = damageBoost;
        temporarySpeedModifier = movementSpeedBoost;


        UpdateCharacterStats();
    }
    public void EndBerserk()
    {
        ResetBersekModifers();
        UpdateCharacterStats();
    }

    private void ResetBersekModifers()
    {
        temporaryArmorReductionModifier = 1.0f;
        temporaryAttackSpeedModifier = 1.0f;
        temporaryMeleeDamageModifier = 1.0f;
        temporarySpeedModifier = 1.0f;
        damageReduction = baseArmor;
    }

    private float CalculateSpeedModifierFromRelics()
    {
        float increasePercentage = 0.5f;
        float dexterity = playerStats.Dexterity.Value;
        float dexterityBonus = 1 + (dexterity * (increasePercentage / 100f));

        return baseSpeed / dexterityBonus;
    }

    private float CalculateRangedAttackSpeedModifierFromRelics()
    {
        float increasePercentage = 2f;
        float dexterity = playerStats.Dexterity.Value;
        float dexterityBonus = 1 + (dexterity * (increasePercentage / 100f));

        if (dexterity == 0)
            return baseRangedAttackSpeed / 1;
        else
            return baseRangedAttackSpeed / dexterityBonus;


    }

    private float CalculateAttackDamageModifierFromRelics()
    {
        return playerStats.Strength.Value;
    }

    public int CalculateSpellDamageModifierFromRelics()
    {
        return (int)playerStats.Intelligence.Value * 2;
    }

    private float CalculateMaxLucidityModifierFromRelics()
    {
        if (playerStats.Intelligence.Value == 0)
            return originalMaxLucidity;
        else
            return originalMaxLucidity + playerStats.Intelligence.Value * 5;
    }
    public void UpdateCharacterStats()
    {

        float lucidityPercentage = lucidity / maxLucidity;


        speed = CalculateSpeedModifierFromRelics() * temporarySpeedModifier;
        rangedAttackSpeed = CalculateRangedAttackSpeedModifierFromRelics() / temporaryAttackSpeedModifier;
        attackSpeed = baseAttackSpeed / temporaryAttackSpeedModifier;
        meleeDamage = (baseMeleeDamage + CalculateAttackDamageModifierFromRelics())*temporaryMeleeDamageModifier;

        maxLucidity = CalculateMaxLucidityModifierFromRelics();
        lucidity = maxLucidity * lucidityPercentage;
        lucidityPostProcess.UpdateLucidityMask(lucidity);

        damageReduction *= temporaryArmorReductionModifier;

        controller.MoveSpeed = speed;

        Debug.Log("Melee Damage: " + meleeDamage + ", attack speed: " + attackSpeed + ", movement speed: " + speed + ", armor amount: " + damageReduction);
    }

    public void TakeDamage(float damage)
    {
        // Ensure lucidity is within the valid range
        lucidity = Mathf.Clamp(lucidity - (damage * damageReduction), 0f, maxLucidity);

        lucidityPostProcess.UpdateLucidityMask(lucidity);

        if (lucidity <= 0)
        {
            gameObject.SetActive(false);
            isDead = true;
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

    public void FixedUpdate()
    {

        float lucidityProcentage = lucidity / maxLucidity * 100;
        //Debug.Log("Lucidity %: " +lucidityProcentage);
        AudioManager.instance.musicEventInstance.setParameterByName("Lucidity", lucidityProcentage);

    }
}
