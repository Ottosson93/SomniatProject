using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    PickUpScript pickUpScript;
    private SpellAttackSystem playerLuciditySystem;
    public Spell spell;


    // Start is called before the first frame update
    void Start()
    {
        playerLuciditySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellAttackSystem>();
        pickUpScript = GetComponent<PickUpScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpScript.PickUp) // Dont know what to name it but i simply remade Atalay's update script to return a bool 
            ConsumeSpellItem(playerLuciditySystem, spell);       // So that i could use it in this class for my Equip() script.

    }
}
