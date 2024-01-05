using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    public EventInstance musicEventInstance;

    public int enemiesEngaged = 0;

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

    public void RestartMusic(EventReference musicEventReference)
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
    public void AddEnemyEngage()
    {
        enemiesEngaged++;
        if(enemiesEngaged == 1)
        {
            musicEventInstance.setParameterByName("Combat", 1f);
        }
    }
    public void removeEnemyEngage()
    {
        enemiesEngaged--;
        if (enemiesEngaged == 0)
        {
            musicEventInstance.setParameterByName("Combat", 0f);
        }
    }

    private void OnDestroy()
    {
        CleanUpAudio();
    }
}
