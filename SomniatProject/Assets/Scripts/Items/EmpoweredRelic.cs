using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmpoweredRelic : MonoBehaviour
{
    public  List<Effect> effects = new List<Effect>();
    public List<EffectType> effectTypes;
    public float speedDuration = 10f;
    public float movespeed = 10f;
    public float healAmount = 10f;
    public Spell spell;
    private Player player;
    private PickUpScript pickUpScript;

    void Start()
    {
        pickUpScript = GetComponent<PickUpScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for(int i = 0; i<effectTypes.Count; ++i)
        {
            if (effectTypes[i] == EffectType.MoveSpeed)
                effects.Add(new Effect_MoveSpeed(speedDuration,movespeed,player));
            else if (effectTypes[i] == EffectType.Healing)
                effects.Add(new Effect_Healing(healAmount, player));
            else if (effectTypes[i] == EffectType.Status)
                effects.Add(new Effect_Status(spell, player, new Vector3()));
        }
    }


    private void Update()
    {
        if (pickUpScript.PickUpButtonPressed)
        {
            player.empoweredRelic = this;
            gameObject.SetActive(false);
        }

    }

}
