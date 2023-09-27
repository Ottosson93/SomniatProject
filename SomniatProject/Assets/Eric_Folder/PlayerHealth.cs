using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerHealth : Health
{
    [SerializeField] int maxHealth = 100;


    public Lucidity lucidity;


    private Animator animator;
    [SerializeField] GameObject playerDieExplosion;
    [SerializeField] float healthDamageAnimation;
    [SerializeField] float waitForAnimationTimer;
    [SerializeField] float waitForEndScreenTimer;
    void Start()
    {
        waitForAnimationTimer = 0.3f;
        waitForEndScreenTimer = 1.1f;
        health = maxHealth;
        lucidity.SetMaxLucidity(maxHealth);


        if (GameObject.Find("PassableObject") != null)
        {
            //GameObject.Find("PassableObject").GetComponent<PassingScript>().GetHealth();
        }
       
        animator = GetComponent<Animator>();
        healthDamageAnimation = health;
    }

    IEnumerator GettingHurt(float hurtAnimationDelay)
    {
        // animator.Play("Red_Solider_light_Hurt");
        yield return new WaitForSeconds(hurtAnimationDelay);
        healthDamageAnimation = health;
    }
    private void Update()
    {

        if (healthDamageAnimation > health)
        {
            StartCoroutine(GettingHurt(.7f));
        }
        else
        {
            //animator.Play("Red_Solider_light_Walking");
        }

    }
    public override void TakeDamage(int damage)
    {
        health -= damage;

        lucidity.SetLucidity(health);

        if (health <= 0)
        {
            StartCoroutine(Die());
            gameObject.SetActive(false);
            
        }
    }

    public IEnumerator Die()
    {
        Instantiate(playerDieExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitForAnimationTimer);
        // animator.Play("DieAnimation");
        yield return new WaitForSeconds(waitForEndScreenTimer);
        // FindObjectOfType<LevelLoader>().LoadGameOver();
    }
    public void Heal(int amountHealed)
    {
        health += amountHealed;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

