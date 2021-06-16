using JetBrains.Annotations;
using UnityEngine;
using TMPro;

/// <summary>
/// Tracks collected feathers
/// </summary>
public class FeatherTracker : AListenerEnabler, IRestartable
{
    [SerializeField] private TextMeshProUGUI ingameFeatherCount;
    public int CollectedFeathers { get; private set; }
    
    private void Start() => RegisterWithHandler();

    [UsedImplicitly]
    public void OnFeatherCollected() => ingameFeatherCount.text = $"{++CollectedFeathers}";

    public void Restart()
    {
        CollectedFeathers = 0;
        ingameFeatherCount.text = "0";
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
