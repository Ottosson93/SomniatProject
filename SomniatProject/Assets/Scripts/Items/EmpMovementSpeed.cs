using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpMovementSpeed : Emp
{
    float duration = 2f;
    float speedIncrease = 5f;
    Player player;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override async void Run()
    {
        await player.MovementIncrease(speedIncrease,duration);
        Debug.Log("Done with Run");
    }


}
