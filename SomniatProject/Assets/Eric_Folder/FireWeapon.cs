using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder
{

    public class FireWeapon : MonoBehaviour
    {
        [SerializeField] Transform firePoint;
        private bool isShooting;
        [SerializeField] float bulletForce = 100f;
        [SerializeField] int WeaponDamage = 10;
        [Range(0.01f, 1f)][SerializeField] float rateOfFire = 0.2f;
        [SerializeField] float accuracy = 0f;
        Coroutine firingCoroutine;

        [SerializeField]Camera camera;
        [SerializeField] GameObject bulletTrailLine;
        [SerializeField] GameObject muzzleFlashPrefab;
        [SerializeField] GameObject crosshair;
        private GameObject muzzleFlashObject;
        private MuzzleFlash muzzelFlashScript;
        [SerializeField] AudioSource audioSource;


        //Test
        [SerializeField] float projectileForce = 20f;
        [SerializeField] GameObject projectilePrefab;

        //GamePauser gamePauser;

        private void Start()
        {
            isShooting = false;
            //camera = FindObjectOfType<Camera>();
           // muzzleFlashObject = Instantiate(muzzleFlashPrefab, firePoint);
            //muzzelFlashScript = muzzleFlashObject.GetComponent<MuzzleFlash>();
            //    gamePauser = FindObjectOfType<GamePauser>();
            
            crosshair = Instantiate(crosshair);
        }

        void Update()
        {

            Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
            //Prevents the bullet trail from going to far when not hitting anything
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
            crosshair.transform.position = mouseWorldPos;


            //     if (!gamePauser.InMenu) //Man kan inte skjuta när man är i shoppen
            {
                bool buttonDownRight = UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame;
                bool buttonUpRight = UnityEngine.InputSystem.Mouse.current.rightButton.wasReleasedThisFrame;
               
                if (UnityEngine.InputSystem.Mouse.current.rightButton.isPressed)
                {
                    Debug.Log("Right Button is pressed");
                }
                
                if (buttonDownRight && isShooting == false)
                {
                    Debug.Log("Fire1");
                    isShooting = true;
                   // InstantiateBullet();
                    InstantiateBullet_Projectile();
                    //Fire();
                    //firingCoroutine = StartCoroutine(Fire());
                }
                else if (buttonDownRight && isShooting == true)
                {
                    //StopCoroutine(firingCoroutine);
                    isShooting = false;
                }
                else if (buttonUpRight)
                {
                    //StopCoroutine(firingCoroutine);
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
               // AnimateMuzzleFlash();

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

            Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
            //Prevents the bullet trail from going to far when not hitting anything
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(mousePos);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, new Vector3(mouseWorldPos.x, transform.position.y, mouseWorldPos.z));


            /*RaycastHit[] hitInfo = Physics.RaycastAll(firePoint.position, firePoint.forward);
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
                    lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * hit.distance); // KAnske behöver firepoint.forward.
                }

            }
            else*/
            //{


            //     Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y%Screen.height, 0) ;
            //    //Prevents the bullet trail from going to far when not hitting anything
            //    Vector3 mouseWorldPos = camera.ScreenToWorldPoint(mousePos);

            //    lineRenderer.SetPosition(0, firePoint.position);
            //    lineRenderer.SetPosition(1, new Vector3(mouseWorldPos.x,transform.position.y, mouseWorldPos.z));
            //}



        }
            private void InstantiateBullet_Projectile()
            {
            Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
            //Prevents the bullet trail from going to far when not hitting anything
            Ray ray = camera.ScreenPointToRay(mousePos);
            Vector3 targetPos = new Vector3();
            if(Physics.Raycast(ray, out RaycastHit raycast))
            {
                targetPos = raycast.point;
                Debug.Log("Raycasted");
            }


            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damage = WeaponDamage;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Vector3 dir = new Vector3(targetPos.x-firePoint.position.x,0,targetPos.z - firePoint.position.z).normalized;
                rb.AddForce(dir * projectileForce, ForceMode.Impulse); // KAnske behöver firepoint.forward istället.
                
                Debug.Log("Mouse_screen_pos: " + mousePos.x + " "+mousePos.y+" " + mousePos.z + "  Mouse_world_pos: " + camera.ScreenToWorldPoint(mousePos).x + " "+ camera.ScreenToWorldPoint(mousePos).y+" " + camera.ScreenToWorldPoint(mousePos).z);
            }
    }

}