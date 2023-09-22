using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder
{
    public class DestructibleHealth : MonoBehaviour
    {
        [SerializeField] int health = 10;
        Animator animation;
        [SerializeField] Sprite deathSprite;
        [SerializeField] GameObject loot;

        private void Start()
        {
            animation = GetComponent<Animator>();
        }
        public void TakingDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                HandleDeath();
            }
            else
            {
                animation.Play("TakeDamage");
            }
        }

        public void HandleDeath()
        {
            UpdateDeathGraphics();
            RemoveColliders();
            SpawnLoot();
        }

        public void UpdateDeathGraphics()
        {
            animation.Play("Die");
            gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;

        }

        public void RemoveColliders()
        {
            if (gameObject.GetComponent<SphereCollider>() != null)
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
            if (gameObject.GetComponent<CapsuleCollider>() != null)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            if (gameObject.GetComponent<BoxCollider>() != null)
            {
                gameObject.GetComponent <BoxCollider>().enabled = false;
            }
        }

        public void SpawnLoot()
        {
            if (loot != null)
            {
                Instantiate(loot, transform.position, Quaternion.identity);
            }
        }
    }
}
