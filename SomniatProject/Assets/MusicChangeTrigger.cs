using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            AudioManager.instance.InitializeMusic(SoundEvents.instance.death);

            //AudioManager.instance.PlaySingleSFX(SoundEvents.instance.death, collider.transform.position);
        }
    }
}
