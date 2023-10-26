using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Effect_MoveSpeed : Effect
{
    public float duration;
    public float speedIncrease;
    private float time_passed;
 
   public Effect_MoveSpeed()
    {
        duration = 2f;
        speedIncrease = 10f;
        time_passed = 0;
        type = EffectType.MoveSpeed;
    }

    public Effect_MoveSpeed(float _duration,float _speedIncrease, Player _player)
    {
        duration = _duration;
        speedIncrease = _speedIncrease;
        player = _player;
        type = EffectType.MoveSpeed;
    }


    public async override void Run()
    {
        player.flatSpeed += speedIncrease;
        player.controller.MoveSpeed = player.CalculateSpeed();
        Debug.Log("PlayerSpeed " + player.CalculateSpeed());
        await Timer(duration);
        player.flatSpeed -= speedIncrease;
        player.controller.MoveSpeed = player.CalculateSpeed();
        Debug.Log("PlayerSpeed " + player.CalculateSpeed());
    }

    async Task Timer(float duration)
    {
        int delayTime = 10;

        while (time_passed < duration)
        {
            await Task.Delay(delayTime);
            time_passed += Time.deltaTime;
        }
        time_passed = 0;
        return;
    }


}

