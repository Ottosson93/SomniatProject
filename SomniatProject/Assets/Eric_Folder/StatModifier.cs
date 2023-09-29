using UnityEngine;

public class StatModifier : MonoBehaviour
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


    private void Start()
    {
        if(_order == 0)
        {
            _order = (int)statModType;
        }
        Source = GetComponent<Relic>();
    }


    public StatModType statModType;
    public  object Source;
    public int Order { get { return _order; } }
    private int _order;


    public  float Value;
    public StatModifier(float value, StatModType type, int order, object src)
    {
        statModType = type;
        Value = value;
        _order = order;
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
