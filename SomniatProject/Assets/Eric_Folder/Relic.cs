using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Eric_folder;

public class Relic : Item
{
    private Player player;
    public StatModifier.CharacterStatType[] statType;
    public StatModifier.StatModType[] modType;
    public float[] value;
    [SerializeField] PickUpScript pickUpScript;

    void CreateStats()
    {
        Debug.Log("Createstats run");

        stat_arr = new StatModifier[value.Length];

        for (int i = 0; i < stat_arr.Length; i++)
        {
            stat_arr[i] = new StatModifier(value[i], modType[i], this, statType[i]);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CreateStats();
        player = FindObjectOfType<Player>();

    }

    private void Update()
    {
        if (pickUpScript.PickUp().Result) // Dont know what to name it but i simply remade Atalay's update script to return a bool 
            Equip(player);                 // So that i could use it in this class for my Equip() script.
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            Equip(p);
        }
    }
}
