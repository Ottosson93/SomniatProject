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


    public float newSpeed;

    public ThirdPersonController controller;
    private LuciditySlider luciditySlider;

    public EmpoweredRelic empoweredRelic;

    void Start()
    {
        Strength = new CharacterStat();
        Dexterity = new CharacterStat();
        Intelligence = new CharacterStat();
        controller = GetComponent<ThirdPersonController>();

        speed = baseSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;


        maxLucidity = CalculateMaxLucity();
        lucidity = maxLucidity;
        controller.MoveSpeed = CalculateSpeed();

        luciditySlider = GetComponent<LuciditySlider>();
        luciditySlider.SetMaxLucidity(lucidity);
        
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
        return newSpeed = baseSpeed * (1 + (Dexterity.Value / baseSpeed))+flatSpeed ;
    }

   public float CalculateAttackSpeed()
    {
        return baseAttackSpeed * (1 + (Dexterity.Value));
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
        if(Strength.Value == 0)
        {
            return 20f;
        }
        return Strength.Value * 20;
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
        luciditySlider.SetMaxLucidity(maxLucidity);
        lucidity = adjuster * maxLucidity;
        GetComponent<ThirdPersonController>().MoveSpeed = CalculateSpeed();

    }

    public void TakeDamage(float damage)
    {
        lucidity -= (damage * damageReduction);
        lucidity = Mathf.Clamp(lucidity, 0f, maxLucidity);  // Ensure lucidity is within the valid range

        luciditySlider.SetLucidity(lucidity);

        if (lucidity <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Heal(float amountHealed)
    {
        lucidity += amountHealed;

        luciditySlider.SetLucidity(lucidity);

        if (lucidity > maxLucidity)
        {
            lucidity = maxLucidity;
        }
    }

    public void Equip(RelicData d)
    {
        foreach(StatModifier s in d.GetModifiers())
        {
            switch (s.characterStatType)
            {
                case StatModifier.CharacterStatType.Dexterity :
                    Dexterity.AddModifier(s);
                    break;
                case StatModifier.CharacterStatType.Strength:
                    Strength.AddModifier(s);
                    break;
                case StatModifier.CharacterStatType.Intelligence:
                    Intelligence.AddModifier(s);
                    break;      
            }
            d.relicQuantity++;
        }
        UpdateCharacterStats();
    }
}