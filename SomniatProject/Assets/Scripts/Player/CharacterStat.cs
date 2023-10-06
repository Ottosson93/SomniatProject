
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

[Serializable]
public class CharacterStat 
{
    public float BaseValue;

    public float Value
    {
        get
        {
            if (isDirty)
            {
                _value = CalculateFinalValue();
                isDirty = false;

            }
            return _value;
        }
    }

    private readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    private bool isDirty = true;
    private float _value;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharacterStat(float baseValue)
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);

    }

    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
        isDirty = true;
            return true;
        }
        return false;
    }

    public bool RemoveAllModifiersFromSource(object src)
    {
        bool didRemove = false;
        for(int i= statModifiers.Count- 1; i>=0; --i)
        {
            if(statModifiers[i].Source == src)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }


    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for(int i = 0; i<statModifiers.Count; ++i)
        {
            StatModifier mod = statModifiers[i];
            if (mod.statModType == StatModifier.StatModType.Flat)
                finalValue += statModifiers[i].Value;
            else if (mod.statModType == StatModifier.StatModType.PercentAdd)
            {
                sumPercentAdd += mod.Value;
                if(i+1>= statModifiers.Count || statModifiers[i+1].statModType != StatModifier.StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if(mod.statModType == StatModifier.StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);

    }


}
