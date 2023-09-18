using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Eric_folder
{

public class FireWeapon_Reload : MonoBehaviour
{
        [SerializeField] Transform firePoint;
        [SerializeField] AudioSource shootSound;

        [SerializeField] float projectileForce = 20f;
        [SerializeField] int WeaponDamage = 10;
        [SerializeField] float accuracy = 0f;
        [Range(0.01f, 1f)] public float rateOfFire = 0.2f;

        [SerializeField] GameObject projectilePrefab;
        private GameObject ammoUI;
        Coroutine firingCoroutine;
        int ammo;
        bool isShooting = false;
      //  GamePauser gamePauser;

        public int Ammo
        {
            get => ammo;
            set => ammo = value;
        }

        private void Start()
        {
            ammoUI = GameObject.Find("Ammo");
            SetAmmo();
            //gamePauser = FindObjectOfType<GamePauser>();
        }

        public void SetAmmo()
        {
            switch (gameObject.name)
            {
                case "SW 0":
                    ammo = 5;
                    break;
                case "SW 1":
                    ammo = 5;
                    break;
                case "SW 2":
                    ammo = 30;
                    break;
                case "SW 3":
                    ammo = 20;
                    break;
                case "SW 4":
                    ammo = 15;
                    break;
                case "SW 5":
                    ammo = 15;
                    break;
                case "SW 6":
                    ammo = 15;
                    break;
                case "SW 7":
                    ammo = 15;
                    break;
                case "SW 8":
                    ammo = 15;
                    break;
                case "SW 9":
                    ammo = 15;
                    break;
                case "SW 10":
                    ammo = 15;
                    break;
                case "SW 11":
                    ammo = 15;
                    break;
                case "SW 12":
                    ammo = 15;
                    break;
                case "SW 13":
                    ammo = 15;
                    break;
                default:
                    break;
            }

            ammoUI.GetComponent<TMPro.TextMeshProUGUI>().text = ammo.ToString();
        }

        public void AddAmmo(int aValue)
        {
            ammo += aValue;
            ammoUI.GetComponent<TMPro.TextMeshProUGUI>().text = ammo.ToString();
        }

        void Update()
        {
   //         if (ammo > 0 && !gamePauser.InMenu) //You can't shoot while in the shop.
            {

                if (Input.GetButtonDown("Fire2") && isShooting == false)
                {
                    isShooting = true;
                    Fire();
                    firingCoroutine = StartCoroutine(Fire());
                }
                else if (Input.GetButtonDown("Fire2") && isShooting == true)
                {
                    StopCoroutine(firingCoroutine);
                    isShooting = false;
                }
            }
            if (Input.GetButtonUp("Fire2") && firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                isShooting = false;
            }
        }

        IEnumerator Fire()
        {
            while (true)
            {
                InstantiateBulletAndUpdateUI();
                if (ammo == 0)
                {
                    DropWeapon();
                }
                if (shootSound != null)
                {
                    shootSound.Play();
                }
                yield return new WaitForSeconds(rateOfFire);
            }
        }
        private void DropWeapon()
        {
            SetAmmo();
            gameObject.SetActive(false);

            GameObject player = GameObject.FindWithTag("Player");
 //           PickUpWeapon pickUpWeapon = player.GetComponent<PickUpWeapon>();
 //           pickUpWeapon.weaponID = -1; //Is set to -1 same as using null.

            ammoUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }

        private void InstantiateBulletAndUpdateUI()
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.up * projectileForce, ForceMode.Impulse); // KAnske behöver firepoint.forward istället.
            ammo--;
            ammoUI.GetComponent<TMPro.TextMeshProUGUI>().text = ammo.ToString();
        }
    }

}