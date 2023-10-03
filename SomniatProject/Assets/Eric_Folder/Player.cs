using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

    public class Player : MonoBehaviour
    {
        [SerializeField] public float maxLucidity = 100;
        public CharacterStat Strength;
        public CharacterStat Dexterity;
        public CharacterStat Intelligence;
        private float speed;
        public float lucidity;
        private float meleeDamage;
        private float attackSpeed;

        private ThirdPersonController controller;
        
        void Start()
        {
            Strength = new CharacterStat();
            Dexterity = new CharacterStat();
            Intelligence = new CharacterStat();
            controller = GetComponent<ThirdPersonController>();
            lucidity = maxLucidity;

            Dexterity.BaseValue = 2.0f;
            Strength.BaseValue = 5.0f;
            Intelligence.BaseValue = 1.0f;


            controller.MoveSpeed = Dexterity.Value;
            lucidity = Strength.Value;
        }

        public void UpdateCharacterStats()
        {
            lucidity = Strength.Value;
            GetComponent<ThirdPersonController>().MoveSpeed= Dexterity.Value;

            Debug.Log("Updating health + movementspeed : " + lucidity + " " + Dexterity.Value);
        }

        public  void TakeDamage(float damage)
        {
            lucidity -= damage;
            lucidity = Mathf.Clamp(lucidity, 0f, maxLucidity);  // Ensure lucidity is within the valid range


            if (lucidity <= 0)
            {
               gameObject.SetActive(false);
            }
        }

        public void Heal(float amountHealed)
        {
            lucidity += amountHealed;
            if (lucidity > maxLucidity)
            {
                lucidity = maxLucidity;
            }
        }
    }