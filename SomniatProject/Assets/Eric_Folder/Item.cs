using System;
using Assets.Eric_folder;

[Serializable]
public class Item
{
    protected StatModifier[] stat_arr;
     


   public Item()
    {
        stat_arr = new StatModifier[]
        {
            new StatModifier(1.0f,StatModifier.StatModType.Flat,this,StatModifier.CharacterStatType.Dexterity)
        };
    }
    public Item(StatModifier[] statModifiers)
    {
        stat_arr = statModifiers;
    }


    public virtual void Equip(PlayerHealth c)
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

    public void Unequip(PlayerHealth c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
        c.Dexterity.RemoveAllModifiersFromSource(this);
        c.Intelligence.RemoveAllModifiersFromSource(this);
    }
}

