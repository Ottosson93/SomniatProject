

public class StatModifier 
{
    public enum CharacterStatType
    {
        Dexterity,
        Strength,
        Intelligence
    };
    public CharacterStatType characterStatType;

    public enum StatModType
    {
        Flat,
        PercentAdd,
        PercentMult,
    };

    public readonly object Source;
    public readonly int Order;

    public StatModType statModType;

    public readonly float Value;
    public StatModifier(float value, StatModType type, int order, object src)
    {
        statModType = type;
        Value = value;
        Order = order;
        Source = src;
    }

    public StatModifier(float value, StatModType type) : this(value, type, (int)type)
    {

    }
   public StatModifier(float value, StatModType type, int order) : this(value, type, order, null)
    {

    }
    public StatModifier(float value, StatModType type, object src) : this(value, type, (int)type, src)
    {

    }
    public StatModifier(float value, StatModType type, object src, CharacterStatType _characterStatType)
    {
        Value = value;
        statModType = type;
        Source = src;
        characterStatType = _characterStatType;
    }


}
