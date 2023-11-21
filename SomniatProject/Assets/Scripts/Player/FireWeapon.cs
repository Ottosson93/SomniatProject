using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;



public class FireWeapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    private bool isShooting;
    [SerializeField] float bulletForce;
    [SerializeField] int WeaponDamage;
    [Range(0.01f, 1f)] [SerializeField] float rateOfFire;
    [SerializeField] float accuracy;
    Coroutine firingCoroutine;
    bool canShoot = true;
    Player player;

    Hud_Attack hud_ranged_attack;


    [SerializeField] Camera camera;
    [SerializeField] GameObject crosshair;
    [SerializeField] AudioSource audioSource;

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
        player = GetComponent<Player>();
    }

    void Update()
    {

        Vector3 mousePos = new Vector3(UnityEngine.InputSystem.Mouse.current.position.value.x % UnityEngine.Screen.width, UnityEngine.InputSystem.Mouse.current.position.value.y % Screen.height, 0);
        //Prevents the bullet trail from going to far when not hitting anything
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
        //crosshair.transform.position = mouseWorldPos;


        //     if (!gamePauser.InMenu) //Man kan inte skjuta n�r man �r i shoppen
        {
            bool isRightbtnPressed = UnityEngine.InputSystem.Mouse.current.rightButton.isPressed;

            if (isRightbtnPressed)//&& isShooting == false)
            {
                Fire();
            }
        }
    }

    async void Fire()
    {

        if (!canShoot)
            return;
        canShoot = false;

        hud_ranged_attack.Run();

        if (audioSource != null)
        {
            audioSource.Play();
        }
        InstantiateBullet_Projectile();

        int cooldownMs = (int)(1000 / player.CalculateAttackSpeed());
        await Task.Delay(cooldownMs);
        canShoot = true;

    }
    public void InstantiateBullet_Projectile()
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
        //bullet.transform.Translate(Vector3.forward * bulletForce * Time.deltaTime);
        bullet.GetComponent<Bullet>().damage = WeaponDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(targetPos.x - firePoint.position.x, 0, targetPos.z - firePoint.position.z).normalized;
        rb.mass = 5;
        rb.AddForce(dir * bulletForce, ForceMode.Impulse); 


    }
}