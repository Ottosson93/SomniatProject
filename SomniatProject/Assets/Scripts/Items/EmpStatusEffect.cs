using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpStatusEffect : Emp
{
    [SerializeField] Spell spell;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();   
    }

    public override void Run()
    {
        if (spell == null)
            return;
        player.mySpell = spell;
    }

}
