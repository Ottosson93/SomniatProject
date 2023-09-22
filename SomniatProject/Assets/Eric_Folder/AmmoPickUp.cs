using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Eric_folder
{
    public class AmmoPickUp : MonoBehaviour
    {

        [SerializeField] int value = 10;
        [SerializeField] int scoreValue = 20;
        private void OnTriggerEnter3D(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameObject weapon = GameObject.FindGameObjectWithTag("ActiveWeapon");
                //weapon.GetComponent<FireSideWeapon>().AddAmmo(value);
                //PlayerScore playerScore = FindObjectOfType<PlayerScore>();
                // playerScore.AddToScore(scoreValue);
                Destroy(gameObject);
            }
        }
    }

}