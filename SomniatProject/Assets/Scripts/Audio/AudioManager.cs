using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance; 

    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
         
    }

    private void Start()
    {
        InitializeMusic(SoundEvents.instance.music);
        //AudioManager.instance.PlaySingleSFX(SoundEvents.instance.death, new Vector3(0,0,0));
    }
    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
        
    }

    public void PlaySingleSFX(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventinstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventinstance);
        return eventinstance;
    }

    public void CleanUpAudio()
    {
        foreach (EventInstance inst in eventInstances)
        {
            inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            inst.release();
        }
    }

    private void OnDestroy()
    {
        CleanUpAudio();
    }
}