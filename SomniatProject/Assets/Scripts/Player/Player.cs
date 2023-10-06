using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class Player : MonoBehaviour
{
    public float maxLucidity = 100;
    public float lucidity;
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelligence;
    public readonly float baseSpeed = 2.0f;
    public readonly float baseMeleeDamage = 5.0f;
    public readonly float baseAttackSpeed = 1.0f;
    private float speed;
    private float meleeDamage;
    private float attackSpeed;

    private ThirdPersonController controller;
    private Lucidity luciditySlider;




    void Start()
    {
        Strength = new CharacterStat();
        Dexterity = new CharacterStat();
        Intelligence = new CharacterStat();
        controller = GetComponent<ThirdPersonController>();
        lucidity = maxLucidity;

        speed = baseSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;

        Dexterity.BaseValue = 1.0f;
        Strength.BaseValue = 1.0f;
        Intelligence.BaseValue = 1.0f;



        controller.MoveSpeed = CalculateSpeed();
        lucidity = Strength.Value;

        luciditySlider = GetComponent<Lucidity>();
        luciditySlider.SetMaxLucidity(lucidity);
    }

    float CalculateSpeed()
    {
        return baseSpeed * (1 + (Dexterity.Value / baseSpeed));
    }

    float CalculateAttackSpeed()
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



    public void UpdateCharacterStats()
    {
        lucidity = Strength.Value;
        luciditySlider.SetMaxLucidity(lucidity);
        GetComponent<ThirdPersonController>().MoveSpeed = Dexterity.Value;


        Debug.Log("Updating health + movementspeed : " + lucidity + " " + Dexterity.Value);
    }

    public void TakeDamage(float damage)
    {
        lucidity -= damage;
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
}