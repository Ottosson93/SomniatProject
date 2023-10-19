using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using StarterAssets;

public class DashIconScript : MonoBehaviour
{
    Image adjustableImage;
    float value = 0;
    float cooldown = 2f;
    bool onCooldown = true;
    
    private void Start()
    {
        adjustableImage = GameObject.Find("Dash_Filled").GetComponent<Image>();
        cooldown = FindObjectOfType<ThirdPersonController>().dashingCooldown;
    }


    public async void Dash()
    {
        if (adjustableImage == null)
            return;

        adjustableImage.fillAmount = 0;
        await refill();
    }

    async Task refill()
    {
        int delayTime = 50;
        while (adjustableImage.fillAmount < 1)
        {
            await Task.Delay(delayTime);
            
            adjustableImage.fillAmount = adjustableImage.fillAmount + Time.deltaTime * (1 / cooldown);
            Debug.Log("Running Refill");
        }
    }


}
