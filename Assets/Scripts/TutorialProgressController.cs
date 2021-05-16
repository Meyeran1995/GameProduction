using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialProgressController : MonoBehaviour
{
    [Header("Content")] 
    [SerializeField] private GameObject playButton;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        playButton.SetActive(false);
        videoPlayer.Prepare();
    }

    [UsedImplicitly]
    public void StopIntro()
    {
        videoPlayer.Stop();
        playButton.SetActive(true);
        videoPlayer.loopPointReached -= WatchForEndOfClip;
    }

    [UsedImplicitly]
    public void StartIntro()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += WatchForEndOfClip;
    }

    private void WatchForEndOfClip(VideoPlayer video)
    {
        playButton.SetActive(true);
        video.loopPointReached -= WatchForEndOfClip;
    }
}
