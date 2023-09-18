using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder
{

    public class FireWeapon : MonoBehaviour
    {
        [SerializeField] Transform firePoint;
        private bool isShooting;
        [SerializeField] float bulletForce = 20f;
        [SerializeField] int WeaponDamage = 10;
        [Range(0.01f, 1f)][SerializeField] float rateOfFire = 0.2f;
        [SerializeField] float accuracy = 0f;
        Coroutine firingCoroutine;

        Camera camera;
        [SerializeField] GameObject bulletTrailLine;
        [SerializeField] GameObject muzzleFlashPrefab;
        private GameObject muzzleFlashObject;
        private MuzzleFlash muzzelFlashScript;
        [SerializeField] AudioSource audioSource;
        //GamePauser gamePauser;

        private void Start()
        {
            isShooting = false;
            camera = FindObjectOfType<Camera>();
            muzzleFlashObject = Instantiate(muzzleFlashPrefab, firePoint);
            muzzelFlashScript = muzzleFlashObject.GetComponent<MuzzleFlash>();
            //    gamePauser = FindObjectOfType<GamePauser>();
        }

        void Update()
        {
            //     if (!gamePauser.InMenu) //Man kan inte skjuta när man är i shoppen
            {
                if (Input.GetButtonDown("Fire1") && isShooting == false)
                {
                    isShooting = true;
                    Fire();
                    firingCoroutine = StartCoroutine(Fire());
                }
                else if (Input.GetButtonDown("Fire1") && isShooting == true)
                {
                    StopCoroutine(firingCoroutine);
                    isShooting = false;
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    StopCoroutine(firingCoroutine);
                    isShooting = false;
                }
            }
        }

        void AnimateMuzzleFlash()
        {
            muzzelFlashScript.PlayAnimation();
        }

        IEnumerator Fire()
        {
            while (true)
            {
                AnimateMuzzleFlash();

                if (audioSource != null)
                {
                    audioSource.Play();
                }
                InstantiateBullet();

                yield return new WaitForSeconds(rateOfFire);
            }
        }

        private void InstantiateBullet()
        {
            GameObject line = Instantiate(bulletTrailLine, firePoint);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            RaycastHit[] hitInfo = Physics.RaycastAll(firePoint.position, firePoint.up);
            if (hitInfo.Length > 0)
            {
                foreach (RaycastHit hit in hitInfo)
                {

                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        hit.transform.gameObject.GetComponent<EnemyHealth>().TakingDamage(WeaponDamage);
                    }
                    if (hit.transform.gameObject.tag == "Destructible")
                    {
                        hit.transform.gameObject.GetComponent<DestructibleHealth>().TakingDamage(WeaponDamage);
                    }
                    //This is the bullet trail
                    lineRenderer.SetPosition(0, firePoint.position);
                    lineRenderer.SetPosition(1, firePoint.position + firePoint.up * hit.distance); // KAnske behöver firepoint.forward.
                }

            }
            else
            {
                //Prevents the bullet trail from going to far when not hitting anything
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, camera.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

}