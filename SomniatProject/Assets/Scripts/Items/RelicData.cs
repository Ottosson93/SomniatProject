using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class RelicData : ScriptableObject
{
    public string title;
    public string description;
    public int price;

    public StatModifier.CharacterStatType[] statType;
    public StatModifier.StatModType[] modType;
    public float[] value;
    public Sprite icon;

    public StatModifier[] GetModifiers()
    {
        StatModifier[] statArray = new StatModifier[value.Length];
        for (int i = 0; i < statArray.Length; i++)
        {
            statArray[i] = new StatModifier(value[i], modType[i], this, statType[i]);
        }

        return statArray;
    }
}
