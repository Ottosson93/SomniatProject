using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

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

    public float originalMeleeDamage;
    public float originalAttackSpeed;
    public float originalSpeed;
    public float originalArmorAmount;

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
        originalMaxLucidity = maxLucidity;
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
        return baseSpeed * (1 + (playerStats.Dexterity.Value / baseSpeed)) + flatSpeed;
    }

    public float CalculateAttackSpeed()
    {
        return baseAttackSpeed / (1 + (playerStats.Dexterity.Value));
    }

    public float CalculateAttackDamage()
    {
        return meleeDamage + (playerStats.Strength.Value);
    }
    public int CalculateSpellDamage()
    {
        return (int)playerStats.Intelligence.Value * 2;
    }

    float CalculateMaxLucity()
    {
        if (playerStats.Intelligence.Value == 0)
            return 1f;
        else
            return playerStats.Intelligence.Value * 5;
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
        maxLucidity = originalMaxLucidity + CalculateMaxLucity();
        lucidity += CalculateMaxLucity();
        lucidityPostProcess.UpdateLucidityMask(lucidity);
        originalAttackSpeed = CalculateAttackSpeed();
        originalMeleeDamage = CalculateAttackDamage();
        GetComponent<ThirdPersonController>().MoveSpeed = CalculateSpeed();
        Debug.Log("Lucidity: " + lucidity + "Max Lucidity: " + originalMaxLucidity + "Attack Speed: " + originalAttackSpeed + "Melee Damage: " + originalMeleeDamage);
    }

    public void TakeDamage(float damage)
    {
        //lucidity -= (damage * damageReduction);
        
        // Ensure lucidity is within the valid range
        lucidity = Mathf.Clamp(lucidity - (damage * damageReduction), 0f, maxLucidity);  

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