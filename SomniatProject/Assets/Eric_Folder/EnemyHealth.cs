using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHealth : Health
{
    [SerializeField] int scoreValue = 10;
     
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject bloodParticle;

        private void Start()
        {
            
        }

        public override void TakeDamage(float damage)
    {
        health -= damage;
        //if (GetComponentInChildren<HealthBar>() != null)
        {
       //     GetComponentInChildren<HealthBar>().hp = health;
        }
        //Instantiate(bloodParticle, transform.position, Quaternion.identity);
        Die();
    }

    GameObject GetMoney()
    {
        int random = Random.Range(0, 10);
        if (random < 5)
        {
            return Resources.Load("BronzePrefab") as GameObject;
        }
        else if (random < 8)
        {
            return Resources.Load("SilverPrefab") as GameObject;
        }
        else
        {
            return Resources.Load("GoldPrefab") as GameObject;
        }
    }

    private void Die()
    {
        if (health <= 0)
        {
            //Instantiate(GetMoney(), transform.position, Quaternion.identity);
            //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 0.5f);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
              //  player.GetComponent<PlayerScore>().AddToScore(scoreValue);
            }
            gameObject.SetActive(false);
            Destroy(gameObject);

        }
        else
        {
            if (hitSound != null)
            {
                hitSound.Play();
            }
        }
    }
}
