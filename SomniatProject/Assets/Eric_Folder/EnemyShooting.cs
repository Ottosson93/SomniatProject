using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Eric_folder
{

    public class EnemyShooting : MonoBehaviour
    {
        [SerializeField] Transform firePoint;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] float bulletForce = 20f;
        [SerializeField] float damage;
        [SerializeField] float shootingRadius;
        [SerializeField] float timeBetweenShots = 2.0f;
        [SerializeField] float attacksPerSec = 1.5f;
        private Transform target;
        private Rigidbody bulletRigidbody;
        private GameObject bullet;

        private Rigidbody rigidbody;
        private Coroutine shootingCoroutine;
        private bool inRange = false;
        private float nextFire = 0f;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            //target = FindObjectOfType<PlayerMovement>().transform;
        }

        private void FixedUpdate()
        {
            UpdatingDirection();
            Shoot();
        }
        void Shoot() // Behaviour tree -->
        {
            if (Vector2.Distance(target.position, rigidbody.transform.position) <= shootingRadius)
            {
                FireContinuously();
                shootingCoroutine = StartCoroutine(FireContinuously());
                inRange = true;
            }
            else if (Vector2.Distance(target.position, rigidbody.transform.position) <= shootingRadius && inRange == true)
            {
                StopCoroutine(shootingCoroutine);
                inRange = false;
            }
        }
        void UpdatingDirection()
        {
            var ePos = rigidbody.transform.position;
            var tPos = target.position;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(tPos.y - ePos.y, tPos.x - ePos.x) - 90f;
            rigidbody.rotation = Quaternion.AngleAxis(angle,Vector3.up);
        }

        IEnumerator FireContinuously()
        {
            while (Vector2.Distance(target.position, rigidbody.transform.position) <= shootingRadius && Time.time > nextFire)
            {
                nextFire = Time.time + 1f / attacksPerSec;
                InstantiateBullet();
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }

        void InstantiateBullet()
        {
            Vector3 dir = (target.position - transform.position).normalized;
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletRigidbody = bullet.GetComponent<Rigidbody>();
            // bullet.GetComponent<EnemyBullet>().Damage = damage;
            bulletRigidbody.velocity = dir * bulletForce;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, shootingRadius);
            //Shooting radius om de behövs för debugging.
        }
    }

}