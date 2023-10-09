using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Threading.Tasks;

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
     float speed;
    public float meleeDamage;
    private float attackSpeed;

    private ThirdPersonController controller;
    private LuciditySlider luciditySlider;

    public bool canHealOnHit = false;
    public Spell mySpell = null;


    public float Speed { get { return speed; } set { speed = value; } }

    public void HealOnHit(int damage)
    {
        if (canHealOnHit)
        {
            float amount = damage * CalculateLifeSteal();
            Heal(amount);
        }
    }


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

    float CalculateLifeSteal()
    {
        return 0.2f*Intelligence.Value*(1+Strength.Value*0.1f);
    }



    public void UpdateCharacterStats()
    {
        maxLucidity = CalculateLucidity();
        luciditySlider.SetMaxLucidity(maxLucidity);
        GetComponent<ThirdPersonController>().MoveSpeed = CalculateSpeed();
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

    public async Task MovementIncrease(float speed, float duration)
    {
        Debug.Log("Started MovementIncrease");
        Speed += speed;
        int waitTime = 50;
        float time = 0;
        while (time < duration)
        {
            await Task.Delay(waitTime);
            time += waitTime;
            Debug.Log("Time: " + time);
        }
        Speed -= speed;
    }
}