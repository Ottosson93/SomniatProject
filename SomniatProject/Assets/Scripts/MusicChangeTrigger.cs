using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [SerializeField] private string parameterName;
    //[SerializeField] SoundEvents eventSound; 
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            if(parameterName == "Idle")
                AudioManager.instance.RestartMusic(SoundEvents.instance.idleMusic);
            else if (parameterName == "Boss")
                AudioManager.instance.RestartMusic(SoundEvents.instance.bossMusic);
            //AudioManager.instance.musicEventInstance.setParameterByName("Lucidity", 20f);
            //AudioManager.instance.PlaySingleSFX(SoundEvents.instance.death, collider.transform.position);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            AudioManager.instance.RestartMusic(SoundEvents.instance.music);
            //AudioManager.instance.musicEventInstance.setParameterByName("Lucidity", 20f);
            //AudioManager.instance.PlaySingleSFX(SoundEvents.instance.death, collider.transform.position);
        }
    }
}
