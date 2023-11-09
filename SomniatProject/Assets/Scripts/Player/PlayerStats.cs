using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelligence;

    private void OnEnable()
    {
        Strength = new CharacterStat();
        Dexterity = new CharacterStat();
        Intelligence = new CharacterStat();
    }
}
