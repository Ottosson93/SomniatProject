using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Eric_folder
{

    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;
        public float Health;
        [SerializeField] Slider healthBar;
        private Animator animator;
        [SerializeField] GameObject playerDieExplosion;
        [SerializeField] float healthDamageAnimation;
        [SerializeField] float waitForAnimationTimer;
        [SerializeField] float waitForEndScreenTimer;
        void Start()
        {
            waitForAnimationTimer = 0.3f;
            waitForEndScreenTimer = 1.1f;
            Health = maxHealth;
            if (GameObject.Find("PassableObject") != null)
            {
              //  GameObject.Find("PassableObject").GetComponent<PassingScript>().GetHealth();
            }
            UpdateHealthBar();
            animator = GetComponent<Animator>();
            healthDamageAnimation = Health;
        }

        private void UpdateHealthBar()
        {
            float precentageHealth = (float)Health / (float)maxHealth;
            float sliderValue = precentageHealth * 3.5f; //3.5 is tested to be the best value for the healthbar.
            healthBar.value = sliderValue;
        }
        IEnumerator GettingHurt(float hurtAnimationDelay)
        {
            animator.Play("Red_Solider_light_Hurt");
            yield return new WaitForSeconds(hurtAnimationDelay);
            healthDamageAnimation = Health;
        }
        private void Update()
        {
            if (healthDamageAnimation > Health)
            {
                StartCoroutine(GettingHurt(.7f));
            }
            else
            {
                animator.Play("Red_Solider_light_Walking");
            }

        }
        public void TakeDamage(float damage)
        {
            Health -= damage;
            UpdateHealthBar();
            if (Health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        public IEnumerator Die()
        {
            Instantiate(playerDieExplosion, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitForAnimationTimer);
            animator.Play("DieAnimation");
            yield return new WaitForSeconds(waitForEndScreenTimer);
            FindObjectOfType<LevelLoader>().LoadGameOver();
        }
        public void Heal(int amountHealed)
        {
            Health += amountHealed;
            if (Health > maxHealth)
            {
                Health = maxHealth;
            }
            UpdateHealthBar();
        }
    }

}