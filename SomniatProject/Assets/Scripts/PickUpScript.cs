using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Random = UnityEngine.Random;
using Assets.Eric_folder;

public class PickUpScript : MonoBehaviour
{
    public DamgeTextPlayer damageTextPlayer;
    public Transform player;
    public float pickUpRange = 1.5F;
    private Transform eKeyPlane;
    private Material material;
    Vector3 distanceToPlayer;
    bool displayKey, playerInRange;



    void Start()
    {
        displayKey = false;
        playerInRange = false;
        eKeyPlane = transform.GetChild(0);
        material = eKeyPlane.GetComponent<MeshRenderer>().material;
        
        
    }

    // Update is called once per frame
    public async Task<bool> PickUp()
    {
        distanceToPlayer = player.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            

            playerInRange = true;

            if (!displayKey && CanIncreaseOpacity)
                await ShowKey();

            if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                damageTextPlayer.AddHealth(Random.Range(10, 100) , transform);
                return true;
            }
        }
        else
        {
            playerInRange = false;

            if (!displayKey && CanDecreaseOpacity)
                await HideKey();
        }
        return false;
    }

    private async Task ShowKey()
    {
        displayKey = true;
        while (CanIncreaseOpacity && playerInRange)
        {
            material.color = IncreaseOpacity;
            await Task.Delay(10);
        }
        displayKey = false;

    }
    private async Task HideKey()
    {
        displayKey = true;
        while (CanDecreaseOpacity && !playerInRange)
        {
            material.color = DecreaseOpacity;
            await Task.Delay(10);
        }
        displayKey = false;
    }

    private Color DecreaseOpacity => CanDecreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a - 0.01F)) : material.color;
    private Color IncreaseOpacity => CanIncreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a + 0.01F)) : material.color;

    private bool CanIncreaseOpacity => material.color.a <= 1;

    private bool CanDecreaseOpacity => material.color.a >= 0.01F;

}