using UnityEngine;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicController : MonoBehaviour
{
    [SerializeField] private MusicPiece[] pieces;

    private void Awake()
    {
        foreach(MusicPiece p in pieces)
        {
            p.Initialize();
        }
    }

    private static EventInstance currentMusicInstance;

    public static void PlayMusicInstance(EventInstance newInstance)
    {
        if(currentMusicInstance.isValid())
            currentMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        currentMusicInstance = newInstance;
        currentMusicInstance.start();
    }
}
