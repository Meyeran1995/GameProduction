using System;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

/// <summary>
/// Tracks collected feathers and elapsed time with personal bests for an active session
/// </summary>
public class StatTracker : AListenerEnabler, IRestartable
{
    [Header("Texts")] 
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI featherText, ingameFeatherCount;

    private float timeElapsed;
    private int bestFeathers, bestTime;
    public int CollectedFeathers { get; private set; }

    //TODO: Maybe use playerprefs for persistent bests
    private void Start() => RegisterWithHandler();

    private void Update() => timeElapsed += Time.deltaTime;

    [UsedImplicitly]
    public void OnFeatherCollected() => ingameFeatherCount.text = $"{++CollectedFeathers}";

    public void ShowStats()
    {
        int newTime = Mathf.RoundToInt(timeElapsed);

        bestFeathers = CollectedFeathers > bestFeathers ? CollectedFeathers : bestFeathers;
        bestTime = newTime < bestTime || bestTime == 0 ? newTime : bestTime;

        TimeSpan newSpan = TimeSpan.FromSeconds(newTime);
        TimeSpan bestSpan = TimeSpan.FromSeconds(bestTime);

        timeText.gameObject.SetActive(true);
        timeText.text = $"Time elapsed: {newSpan.Minutes}min {newSpan.Seconds}s (Best: {bestSpan.Minutes}min {bestSpan.Seconds}s)";
        featherText.gameObject.SetActive(true);
        featherText.text = $"Feathers collected: {CollectedFeathers} (Best: {bestFeathers})";
    }

    public void Restart()
    {
        timeElapsed = 0f;
        CollectedFeathers = 0;
        ingameFeatherCount.text = "0";
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
