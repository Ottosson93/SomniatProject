using System;
using Assets.Eric_folder;
using UnityEngine;

[Serializable]
public abstract class Item : MonoBehaviour
{
    protected StatModifier[] stat_arr;
     
    public void ConsumeSpellItem(SpellAttackSystem spellAttackSystem, Spell spell)
    {
        spellAttackSystem.UpdateSpell(spell);

        Transform parent = GetComponentInParent<Transform>();
        parent.gameObject.SetActive(false);
    }
}

