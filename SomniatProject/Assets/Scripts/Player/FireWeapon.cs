using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireWeapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    private bool isShooting;
    [SerializeField] float bulletForce = 100f;
    [SerializeField] int WeaponDamage = 10;
    [Range(0.01f, 1f)][SerializeField] float rateOfFire = 0.2f;
    [SerializeField] float accuracy = 0f;
    Coroutine firingCoroutine;

    Hud_Attack hud_ranged_attack;


    [SerializeField] Camera camera;
    [SerializeField] GameObject crosshair;
    [SerializeField] AudioSource audioSource;


    [SerializeField] float projectileForce = 20f;
    [SerializeField] GameObject projectilePrefab;


    private void Start()
    {
        isShooting = false;
        //crosshair = Instantiate(crosshair);
        Hud_Attack[] attacks = FindObjectsOfType<Hud_Attack>();
        for (int i = 0; i < attacks.Length; ++i)
        {
            if (attacks[i].attackType == AttackType.Ranged)
                hud_ranged_attack = attacks[i];
        }
    }

    void Update()
    {

        Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
        //Prevents the bullet trail from going to far when not hitting anything
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
        //crosshair.transform.position = mouseWorldPos;


        //     if (!gamePauser.InMenu) //Man kan inte skjuta när man är i shoppen
        {
            bool buttonDownRight = UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame;
            bool buttonUpRight = UnityEngine.InputSystem.Mouse.current.rightButton.wasReleasedThisFrame;

            if (buttonDownRight && isShooting == false)
            {
                Debug.Log("Fire2");
                isShooting = true;

                firingCoroutine = StartCoroutine(Fire());

            }
            else if (buttonDownRight && isShooting == true)
            {
                StopCoroutine(firingCoroutine);
                isShooting = false;
            }
            if (buttonUpRight)
            {
                StopCoroutine(firingCoroutine);
                isShooting = false;
            }
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            hud_ranged_attack.Run();

            if (audioSource != null)
            {
                audioSource.Play();
            }
            InstantiateBullet_Projectile();

            yield return new WaitForSeconds(rateOfFire);
        }
    }
    private void InstantiateBullet_Projectile()
    {
        Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
        //Prevents the bullet trail from going to far when not hitting anything
        Ray ray = camera.ScreenPointToRay(mousePos);
        Vector3 targetPos = new Vector3();
        if (Physics.Raycast(ray, out RaycastHit raycast))
        {
            targetPos = raycast.point;
            Debug.Log("Raycasted");
        }


        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damage = WeaponDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(targetPos.x - firePoint.position.x, 0, targetPos.z - firePoint.position.z).normalized;
        rb.AddForce(dir * projectileForce, ForceMode.Impulse); // KAnske behöver firepoint.forward istället.

        Debug.Log("Mouse_screen_pos: " + mousePos.x + " " + mousePos.y + " " + mousePos.z + "  Mouse_world_pos: " + camera.ScreenToWorldPoint(mousePos).x + " " + camera.ScreenToWorldPoint(mousePos).y + " " + camera.ScreenToWorldPoint(mousePos).z);
    }
}