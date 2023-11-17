using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;

public class PickUpScript : MonoBehaviour
{
    Transform player;
    public float pickUpRange = 1.5F;
    private Transform eKeyPlane;
    private Material material;
    bool displayKey;

    void Start()
    {
        displayKey = false;
        eKeyPlane = transform.GetChild(0);
        material = eKeyPlane.GetComponent<MeshRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public async Task<bool> Update()
    {
        if (InRangeOfPickup)
        {
            if (!displayKey && CanIncreaseOpacity)
                await ChangeKeyOpacity(IncreaseOpacity);
        }
        else
        {
            if (!displayKey && CanDecreaseOpacity)
                await ChangeKeyOpacity(DecreaseOpacity);
        }

        //Debug.Log("pickupscript return false");
        return false;
    }

    private async Task ChangeKeyOpacity(Color targetColor)
    {
        displayKey = true;

        while ((CanIncreaseOpacity && targetColor == IncreaseOpacity) ||
               (CanDecreaseOpacity && targetColor == DecreaseOpacity))
        {
            material.color = targetColor;
            await Task.Delay(2);
        }

        displayKey = false;
    }
    public bool PickUp => InRangeOfPickup ? Keyboard.current.eKey.wasReleasedThisFrame ? true : false : false;
    public bool InRangeOfPickup => (player.position - transform.position).magnitude <= pickUpRange;
    private Color DecreaseOpacity => CanDecreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a - 0.01F)) : material.color;
    private Color IncreaseOpacity => CanIncreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a + 0.01F)) : material.color;
    private bool CanIncreaseOpacity => material.color.a <= 1;
    private bool CanDecreaseOpacity => material.color.a >= 0.01F;

}
