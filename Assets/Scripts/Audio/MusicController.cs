using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    [EventRef] [SerializeField] private string menuMusic;
    [EventRef] [SerializeField] private string gameloopMusic;
    [EventRef] [SerializeField] private string endStingerMusic;
    
    private EventInstance menuMusicInstance;
    private EventInstance gameloopMusicInstance;
    private EventInstance endStingerMusicInstance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(menuMusic);
        gameloopMusicInstance = RuntimeManager.CreateInstance(gameloopMusic);
        endStingerMusicInstance = RuntimeManager.CreateInstance(endStingerMusic);
    }

    // Aktuell noch recht basic -> Wird mit dynamischer musik komplexer, ich versprechs
    public void StartMenuMusic()
    {
        menuMusicInstance.start();
    }

    public void StartGameMusic()
    {
        gameloopMusicInstance.start();
    }

    public void StartEndStringer()
    {
        endStingerMusicInstance.start();
    }
    
    public void StopMenuMusic()
    {
        menuMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void StopGameMusic()
    {
        gameloopMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void StopEndStringer()
    {
        endStingerMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
