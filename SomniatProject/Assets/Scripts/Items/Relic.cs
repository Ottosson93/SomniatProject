using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Relic : Item
{
    private Player player;
    public RelicData relicData;
    PickUpScript pickUpScript;

    void CreateStats()
    {
        stat_arr = relicData.GetModifiers();
    }


    // Start is called before the first frame update
    void Start()
    {
        CreateStats();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        pickUpScript = GetComponent<PickUpScript>();
        pickUpScript.TurnOffGUI();
    }

    private void Update()
    {
        // Dont know what to name it but i simply remade Atalay's update script to return a bool. So that i could use it in this class for my Equip() script. 
        if (pickUpScript.PickUpItem)
        {
            player.Equip(relicData);
            gameObject.SetActive(false);
        }

    }
}
