using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

    public enum AttackType
    {
        Melee,
        Ranged
    };
public class Hud_Attack : MonoBehaviour
{
    public AttackType attackType;
    Image image;
    Color image_color;
    public float duration = 0.05f;
    float time_passed = 0f;

    private void Start()
    {
        image = GetComponent<Image>();
        image_color = image.material.color;
    }

    public async void Run()
    {
        image.color = Color.gray;
        await Timer(duration);
        time_passed = 0;
        image.color = image_color;
    }

    async Task Timer(float duration)
    {
        int delayTime = 10;

        while (time_passed<duration)
        {
            await Task.Delay(delayTime);
            time_passed += Time.deltaTime;
        }
    }


}
