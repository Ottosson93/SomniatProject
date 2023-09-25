using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

namespace Assets.Eric_folder
{

    public class PlayerHealth : Health
    {
        [SerializeField] float maxHealth = 10;
        public CharacterStat Strength;
        public CharacterStat Dexterity;
        public CharacterStat Intelligence;


        
        //[SerializeField] Slider healthBar;
        private Animator animator;
        [SerializeField] GameObject playerDieExplosion;
        [SerializeField] float healthDamageAnimation;
        [SerializeField] float waitForAnimationTimer;
        [SerializeField] float waitForEndScreenTimer;
        void Start()
        {
            Strength = new CharacterStat();
            Dexterity = new CharacterStat();
            Intelligence = new CharacterStat();
            waitForAnimationTimer = 0.3f;
            waitForEndScreenTimer = 1.1f;
            health = maxHealth;

            Dexterity.BaseValue = 2.0f;
            Strength.BaseValue = 5.0f;
            Intelligence.BaseValue = 1.0f;


            ThirdPersonController tpc = GetComponent<ThirdPersonController>();
                tpc.MoveSpeed = Dexterity.Value;
            health = Strength.Value;
            
            if (GameObject.Find("PassableObject") != null)
            {
              //  GameObject.Find("PassableObject").GetComponent<PassingScript>().GetHealth();
            }
           // UpdateHealthBar();
            animator = GetComponent<Animator>();
            healthDamageAnimation = health;
        }

        public void UpdateCharacterStats()
        {
            health = Strength.Value;
            GetComponent<ThirdPersonController>().MoveSpeed= Dexterity.Value;

            Debug.Log("Updating health + movementspeed : " + health + " " + Dexterity.Value);
        }



        /*private void UpdateHealthBar()
        {
            float precentageHealth = (float)Health / (float)maxHealth;
            float sliderValue = precentageHealth * 3.5f; //3.5 is tested to be the best value for the healthbar.
          //  healthBar.value = sliderValue;
        }*/
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
        public override void TakeDamage(float damage)
        {
            health -= damage;
            //UpdateHealthBar();
            if (health <= 0)
            {
               // StartCoroutine(Die());
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
        public void Heal(float amountHealed)
        {
            health += amountHealed;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
           // UpdateHealthBar();
        }
    }

}