using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
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
        if (pickUpScript.PickUpItem)
        {
            player.Heal(Lucidity);
            DestroyItem();
        }

    }
}
