using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmpoweredRelic : MonoBehaviour
{
    public  List<Effect> effects = new List<Effect>();
    public List<EffectType> effectTypes;
    public float speedDuration = 2f;
    public float movespeed = 5f;
    public float healAmount = 10f;
    public Spell spell;
    private Player player;

    void Start()
    {
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
}
