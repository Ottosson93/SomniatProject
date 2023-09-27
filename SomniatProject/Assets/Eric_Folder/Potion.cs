using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Eric_folder;

public class Potion : MonoBehaviour
{

    public float Lucidity;
    PickUpScript pickUpScript;
    Player player;

    void Start()
    {
        pickUpScript = FindObjectOfType<PickUpScript>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (pickUpScript.PickUp().Result)
            player.Heal(Lucidity);

    }
}