using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using StarterAssets;

public class DashIconScript : MonoBehaviour
{
    Image adjustableImage;
    float cooldown = 2f;
    bool isrefilling = false;
    
    private void Start()
    {
        adjustableImage = GameObject.Find("Dash_Filled").GetComponent<Image>();
        cooldown = FindObjectOfType<ThirdPersonController>().dashingCooldown;
    }


    public async void Dash()
    {
        if (adjustableImage == null)
            return;
        cooldown = FindObjectOfType<ThirdPersonController>().dashingCooldown;
        adjustableImage.fillAmount = 0;
        await refill();
    }

    async Task refill()
    {
        if (isrefilling)
            return;
        isrefilling = true;

        float currentTime =Time.realtimeSinceStartup;
        float oldTime=currentTime;

        int delayTime = 100;
        while (adjustableImage.fillAmount < 1)
        {
            currentTime = Time.realtimeSinceStartup;
            float diff = currentTime - oldTime;

            adjustableImage.fillAmount = adjustableImage.fillAmount + (1 / cooldown) * (diff);
            await Task.Delay(delayTime);
            oldTime = currentTime;
        }
        isrefilling=false;
    }


}
