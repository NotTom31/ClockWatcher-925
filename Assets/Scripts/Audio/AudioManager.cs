using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;
    [Range(0, 1)]

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambientBus;
    private Bus sfxBus;

    private string masterBusPath = "bus:/";
    private string musicBusPath = "bus:/Music";
    private string ambientBusPath = "bus:/Ambience";
    private string sfxBusPath = "bus:/SFX";

    private string musicArea = "area";

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        masterBus = RuntimeManager.GetBus(masterBusPath);
        musicBus = RuntimeManager.GetBus(musicBusPath);
        ambientBus = RuntimeManager.GetBus(ambientBusPath);
        sfxBus = RuntimeManager.GetBus(sfxBusPath);

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.ambience);
        InitializeMusic(FMODEvents.instance.music);

    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambientBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }

    /// <summary>
    /// Plays a single audio clip at the world position.
    /// </summary>
    /// <param name="sound">The eventreference that will be played</param>
    /// <param name="worldPos">Location in the world.</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Initalizes and starts the ambience sounds.
    /// </summary>
    /// <param name="ambienceEventReference">The ambience that will be played.</param>
    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    /// <summary>
    /// Initalizes and starts a song.
    /// </summary>
    /// <param name="musicEventReference">The song that will be played.</param>
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    /// <summary>
    /// Initialize into the game an event emitter onto a gameobject.
    /// </summary>
    /// <param name="eventReference">Event Reference that will be emitting.</param>
    /// <param name="emitterGameObject">The Gameobject that would be emiiting from.</param>
    /// <returns></returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    /// <summary>
    /// Creates an instance of the event that will host the audio.
    /// </summary>
    /// <param name="eventReference">The audio reference that will be played.</param>
    /// <returns></returns>
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    /// <summary>
    /// Modifies the ambience parameter.
    /// </summary>
    /// <param name="parameterName">Name of the FMOD parameter.</param>
    /// <param name="parameterValue">New value of the parameter.</param>
    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    /// <summary>
    /// Modify The music area parameter to change the song.
    /// </summary>
    /// <param name="area">The assigned area for that Music Instance.</param>
    public void SetMusicArea(MusicArea area)
    {
        musicEventInstance.setParameterByName(musicArea, (float) area);

    }

    /// <summary>
    /// Cleans up lingering audio after this compenent is destroyed.
    /// </summary>
    private void CleanUp()
    {
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach(StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
