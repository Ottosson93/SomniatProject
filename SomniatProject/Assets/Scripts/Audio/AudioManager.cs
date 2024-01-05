using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0f, 1f)]
    public static float master = 1;
    [Range(0f, 1f)]
    public static float sfx = 1;
    [Range(0f, 1f)]
    public static float music = 1;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;

    private List<EventInstance> eventInstances;
    public EventInstance musicEventInstance;

    public int enemiesEngaged = 0;

    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        eventInstances = new List<EventInstance>();
        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicBus = RuntimeManager.GetBus("bus:/Music");
    }

    private void Start()
    {
        GameObject[] audioManagers = GameObject.FindGameObjectsWithTag("AudioManager");

        if (audioManagers.Length > 1)
        {
            Destroy(gameObject);
            Debug.LogError("Found more than one Audio Manager in the scene");
            return;
        }

        DontDestroyOnLoad(this);

        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        InitializeMusic(SoundEvents.instance.music);

        instance = this;

        //AudioManager.instance.PlaySingleSFX(SoundEvents.instance.death, new Vector3(0,0,0));
    }
    private void FixedUpdate()
    {
        masterBus.setVolume(master);
        sfxBus.setVolume(sfx);
        musicBus.setVolume(music);
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
        if (enemiesEngaged == 1)
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
