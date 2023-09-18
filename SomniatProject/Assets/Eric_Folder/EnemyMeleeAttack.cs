using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder
{

    public class EnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float attacksPerSec = 1f;
        [SerializeField] float attackRadius;
        [SerializeField] Transform attackPoint;
        [SerializeField] LayerMask playerLayer;
        private float nextTimeToFire = 0f;
        private Animator animator;
        private Transform playerPos;

        void Start()
        {
            animator = GetComponent<Animator>();
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        void Update() //Lägg till i behaviourTree istället för här
        {
            Attack();
            float distance = 1.5f;
            if (Vector3.Distance(transform.position, playerPos.position) < distance)
            {
                animator.Play("Alien8_MeleeAttack");
            }
            else
            {
                animator.Play("Alien8_Walking");
            }
        }
        private void Attack() // Lägg också till i BehaviourTree
        {
            if (Time.time > nextTimeToFire)
            {
                Collider[] collidersHit = Physics.OverlapSphere(attackPoint.position, attackRadius, playerLayer);
                if (collidersHit != null)
                {
                    foreach (Collider collider in collidersHit)
                    {
                        if (collider.tag == "Player")
                        {
                            collider.GetComponent<PlayerHealth>().TakeDamage(damage);
                            nextTimeToFire = Time.time + 1f / attacksPerSec;
                        }

                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius); // ritar attack radien för fienden.
        }
    }

}