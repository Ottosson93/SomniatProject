using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Eric_folder
{

    public class Bullet : MonoBehaviour
    {

        public enum Type
        {
            Player,
            Enemy
        };
        public Type type;
        private string targetTag;
        public int damage;
        // Start is called before the first frame update
        void Start()
        {
            switch (type)
            {
                case Type.Player:
                    targetTag = "Enemy";
                    break;
                case Type.Enemy:
                    targetTag = "Player";
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != targetTag)
                return;
            
            if (targetTag == "Player")
                other.GetComponent<Player>().TakeDamage(damage);
            else
                 other.GetComponent<EnemyHealth>().TakeDamage(damage);

            
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}