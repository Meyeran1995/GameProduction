using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectActivator : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private Image effect;
    [SerializeField] private bool showStartingEffectValue;
    [SerializeField] [Range(0f, 1f)] protected float startingEffectStrength;

    protected void Awake() => RegisterWithHandler();

    public void ApplyEffect(float strength) => effect.color = new Color(effect.color.r, effect.color.g, effect.color.b, strength);
    
    [UsedImplicitly] public void ApplyEffect() => effect.color = new Color(effect.color.r, effect.color.g, effect.color.b, 1f);

    public void Restart() => ApplyEffect(startingEffectStrength);

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    protected void OnValidate()
    {
        if (effect == null || !showStartingEffectValue) return;

        ApplyEffect(startingEffectStrength);
    }
}
