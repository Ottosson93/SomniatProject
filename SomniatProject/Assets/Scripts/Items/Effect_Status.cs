using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Status : Effect
{
    Spell spell;
    Vector3 pos;

    public Effect_Status()
    {
        type = EffectType.Status;
    }

   public Effect_Status(Spell _spell, Player _player, Vector3 position)
    {
        spell = _spell;
        player = _player;
        type = EffectType.Status;
        pos = position;
    }

    public override void Run()
    {
        // Do something wtih spell effect..?
    }
}
