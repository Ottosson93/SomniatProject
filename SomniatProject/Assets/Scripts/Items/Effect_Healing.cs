using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Healing : Effect
{
    private float healAmount;

    public Effect_Healing()
    {
        type = EffectType.Healing;
    }

    public Effect_Healing(float _healAmount, Player _player)
    {
        this.healAmount = _healAmount; 
        player = _player;
        type = EffectType.Healing;
    }

    public override void Run()
    {
        player.Heal(healAmount);
    }

}
  