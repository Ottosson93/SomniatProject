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
    private float speed;
    public float meleeDamage;
    private float attackSpeed;

    private ThirdPersonController controller;
    private LuciditySlider luciditySlider;




    void Start()
    {
        Strength = new CharacterStat();
        Dexterity = new CharacterStat();
        Intelligence = new CharacterStat();
        controller = GetComponent<ThirdPersonController>();

        speed = baseSpeed;
        attackSpeed = baseAttackSpeed;
        meleeDamage = baseMeleeDamage;


        maxLucidity = CalculateLucidity();
        lucidity = maxLucidity;
        controller.MoveSpeed = CalculateSpeed();

        luciditySlider = GetComponent<LuciditySlider>();
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
    float CalculateLucidity()
    {
        if(Strength.Value == 0)
        {
            return 20f;
        }
        return Strength.Value * 20;
    }



    public void UpdateCharacterStats()
    {
        maxLucidity = CalculateLucidity();
        luciditySlider.SetMaxLucidity(lucidity);
        GetComponent<ThirdPersonController>().MoveSpeed = CalculateSpeed();


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