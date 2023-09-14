using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    public float timer;
    public float cooldownTime;

 

    public void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
