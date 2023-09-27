using System;
using Assets.Eric_folder;
using UnityEngine;

[Serializable]
public abstract class Item : MonoBehaviour
{
    protected StatModifier[] stat_arr;
     




    public virtual void Equip(Player c)
    {
        for (int i = 0; i < stat_arr.Length; ++i)
        {
            if (stat_arr[i].characterStatType == StatModifier.CharacterStatType.Strength)
            {
                c.Strength.AddModifier(stat_arr[i]);
            }
            else if (stat_arr[i].characterStatType == StatModifier.CharacterStatType.Intelligence)
            {
                c.Intelligence.AddModifier(stat_arr[i]);

            }
            else if (stat_arr[i].characterStatType == StatModifier.CharacterStatType.Dexterity)
                c.Dexterity.AddModifier(stat_arr[i]);

        }
        c.UpdateCharacterStats();
    }

    public void Unequip(Player c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
        c.Dexterity.RemoveAllModifiersFromSource(this);
        c.Intelligence.RemoveAllModifiersFromSource(this);
    }
}

