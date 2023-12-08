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
#if UNITY_EDITOR
using UnityEditor.Timeline.Actions;
#endif

public class PickUpScript : MonoBehaviour
{
    Transform player;
    public float pickUpRange = 1.5F;
    private Transform eKeyPlane;
    private Material material;
    bool displayKey;
    bool showGUI;

    void Start()
    {
        showGUI = true;
        displayKey = false;
        eKeyPlane = transform.GetChild(0);
        material = eKeyPlane.GetComponent<MeshRenderer>().material;
        material.color = new Color(material.color.r, material.color.g, material.color.b, 0.01F);
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public void TurnOffGUI()
    {
        showGUI = false;
    }

    public async Task<bool> Update()
    {
        if (showGUI)
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

            
        }

        return true;
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
    public bool PickUpItem => InRangeOfPickup ? true : false;
    public bool PickUpButtonPressed => InRangeOfPickup ? Keyboard.current.eKey.wasReleasedThisFrame ? true : false : false;
    public bool InRangeOfPickup => (player.position - transform.position).magnitude <= pickUpRange;
    private Color DecreaseOpacity => CanDecreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a - 0.01F)) : material.color;
    private Color IncreaseOpacity => CanIncreaseOpacity ? new Color(material.color.r, material.color.g, material.color.b, (material.color.a + 0.01F)) : material.color;
    private bool CanIncreaseOpacity => material.color.a <= 1;
    private bool CanDecreaseOpacity => material.color.a >= 0.01F;

}
