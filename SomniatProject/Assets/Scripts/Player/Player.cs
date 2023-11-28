using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System;

public class Player : MonoBehaviour
{
    public float maxLucidity;
    public float originalMaxLucidity;
    public float lucidity;
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelligence;
    public readonly float baseSpeed = 2.0f;
    public readonly float baseMeleeDamage = 10f;
    public readonly float baseAttackSpeed = 1.0f;
    public float damageReduction = 1.0f;
    public float flatSpeed = 0;
    public float speed;
    public float meleeDamage;
    public float attackSpeed;


    public float temporaryAttackSpeedModifier;
    public float temporaryMeleeDamageModifier;
    public float temporarySpeedModifier;
    public float temporaryArmorReductionModifier;

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
        lucidityPostProcess = GetComponent<LucidityPostProcess>();

        speed = baseSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;
        lucidity = maxLucidity;

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
        Debug.Log("Berserk values: Armor Reduction" + temporaryArmorReductionModifier + " Attack Speed Boost: " + temporaryAttackSpeedModifier + " Melee Damage Boost " + temporaryMeleeDamageModifier + " Speed Boost " + temporarySpeedModifier);
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
    }

    private float CalculateSpeedModifierFromRelics()
    {
        return baseSpeed * (1 + (playerStats.Dexterity.Value / baseSpeed));
    }

    private float CalculateAttackSpeedModifierFromRelics()
    {
        if (playerStats.Dexterity.Value == 0)
            return baseAttackSpeed / 1;
        else
            return baseAttackSpeed / playerStats.Dexterity.Value;
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
            return 0f;
        else
            return playerStats.Intelligence.Value * 5;
    }

    //public float CalculateSpeed()
    //{
    //    return baseSpeed * (1 + (playerStats.Dexterity.Value / baseSpeed)) + flatSpeed;
    //}

    //public float CalculateAttackSpeed()
    //{
    //    return baseAttackSpeed / (1 + (playerStats.Dexterity.Value));
    //}

    //public float CalculateAttackDamage()
    //{
    //    return playerStats.Strength.Value;
    //}
    //public int CalculateSpellDamage()
    //{
    //    return (int)playerStats.Intelligence.Value * 2;
    //}

    //float CalculateMaxLucity()
    //{
    //    if (playerStats.Intelligence.Value == 0)
    //        return 1f;
    //    else
    //        return playerStats.Intelligence.Value * 5;
    //}

    //public void IncreaseDamage(float amount)
    //{
    //    meleeDamage *= amount;
    //}

    //public void IncreaseAttackSpeed(float amount)
    //{
    //    attackSpeed *= amount;
    //}

    //public void IncreaseSpeed(float amount)
    //{
    //    newSpeed *= amount;
    //}

    //public void ArmorReduction(float amount)
    //{
    //    damageReduction *= amount;
    //}



    public void UpdateCharacterStats()
    {

        float lucidityPercentage = lucidity / maxLucidity;

        speed = (baseSpeed + CalculateSpeedModifierFromRelics()) * temporarySpeedModifier;
        attackSpeed = CalculateAttackSpeedModifierFromRelics() / temporaryAttackSpeedModifier;
        meleeDamage = (baseMeleeDamage + CalculateAttackDamageModifierFromRelics())*temporaryMeleeDamageModifier;

        maxLucidity += CalculateMaxLucidityModifierFromRelics();
        lucidity = maxLucidity * lucidityPercentage;
        lucidityPostProcess.UpdateLucidityMask(lucidity);

        damageReduction *= temporaryArmorReductionModifier;

        controller.MoveSpeed = speed;

        Debug.Log("Lucidity: " + lucidity + " Max Lucidity: " + maxLucidity + " Attack Speed: " + attackSpeed + " Melee Damage: " + meleeDamage + " Speed: " + speed + " Damage Reduction: " + damageReduction);


        //originalAttackSpeed = CalculateAttackSpeed();
        //originalMeleeDamage += CalculateAttackDamage();
        //controller.MoveSpeed = CalculateSpeed();
        //Debug.Log("Lucidity: " + lucidity + "Max Lucidity: " + originalMaxLucidity + "Attack Speed: " + originalAttackSpeed + "Melee Damage: " + originalMeleeDamage);

        //float modifiedAttackSpeed = originalAttackSpeed * temporaryAttackSpeedModifier;
        //float modifiedMeleeDamage = originalMeleeDamage * temporaryMeleeDamageModifier;
        //float modifiedMovementSpeed = baseSpeed * temporarySpeedModifier;
        //float modifiedArmorReduction = originalArmorAmount * temporaryArmorReductionModifier;

        //attackSpeed = modifiedAttackSpeed;
        //meleeDamage = modifiedMeleeDamage;
        //speed = modifiedMovementSpeed;
        //damageReduction = modifiedArmorReduction;
    }

    public void TakeDamage(float damage)
    {       
        lucidity = Mathf.Clamp(lucidity - (damage * damageReduction), 0f, maxLucidity);  // Ensure lucidity is within the valid range

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