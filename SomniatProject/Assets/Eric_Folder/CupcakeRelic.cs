using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeRelic : Relic
{
    public int DexterityGain;

    private void Awake()
    {
        DexterityGain = 2;
        stat_arr = new StatModifier[] { GetDexterityValue()};
        Debug.Log("WHY" + stat_arr[0]);
    }

    public StatModifier GetDexterityValue()
    {
        return new StatModifier(DexterityGain, StatModifier.StatModType.Flat, this, StatModifier.CharacterStatType.Dexterity);
    }

}
