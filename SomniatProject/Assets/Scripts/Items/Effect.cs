using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectType
{
    Healing,
    MoveSpeed,
    Status
};

public abstract class Effect 
{
    public EffectType type;

    protected Player player;


    public abstract void Run();



}
