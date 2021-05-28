using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MusicPiece")]
public class MusicPiece : ScriptableObject
{
    [EventRef] [SerializeField] private string musicName;
    private EventInstance musicInstance;

    public void Initialize()
    {
        musicInstance = RuntimeManager.CreateInstance(musicName);
    }

    public void PlaySolo() => MusicController.PlayMusicInstance(musicInstance);
}
