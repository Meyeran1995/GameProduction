using System;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

/// <summary>
/// Tracks collected feathers and elapsed time with personal bests for an active session
/// </summary>
public class StatTracker : AMultiListenerEnabler, IRestartable
{
    [Header("Texts")] 
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI featherText, ingameFeatherCount;

    private float timeElapsed;
    private int amountOfCollectedFeathers, bestFeathers, bestTime;

    //TODO: Maybe use playerprefs for persistent bests
    private void Start() => RegisterWithHandler();

    private void Update() => timeElapsed += Time.deltaTime;

    [UsedImplicitly]
    public void OnFeatherCollected() => ingameFeatherCount.text = $"{++amountOfCollectedFeathers}";

    [UsedImplicitly]
    public void ShowStats()
    {
        int newTime = Mathf.RoundToInt(timeElapsed);

        bestFeathers = amountOfCollectedFeathers > bestFeathers ? amountOfCollectedFeathers : bestFeathers;
        bestTime = newTime < bestTime || bestTime == 0 ? newTime : bestTime;

        TimeSpan newSpan = TimeSpan.FromSeconds(newTime);
        TimeSpan bestSpan = TimeSpan.FromSeconds(bestTime);

        timeText.text = $"Time elapsed: {newSpan.Minutes}min {newSpan.Seconds}s (Best: {bestSpan.Minutes}min {bestSpan.Seconds}s)";
        featherText.text = $"Feathers collected: {amountOfCollectedFeathers} (Best: {bestFeathers})";
    }

    public void Restart()
    {
        timeElapsed = 0f;
        amountOfCollectedFeathers = 0;
        ingameFeatherCount.text = "0";
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
