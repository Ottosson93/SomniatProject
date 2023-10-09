using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpHealOnHit : Emp
{
    Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void Run()
    {
        player.canHealOnHit = true;
    }


}
