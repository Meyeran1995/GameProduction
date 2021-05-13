using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GlowEffectActivator : AMultiListenerEnabler
{
    [SerializeField] private Image effect;
    [SerializeField] private CircularResourceBar speedBar;
    [SerializeField] [Range(0f, 1f)] private float startingEffectStrength;
    private bool speedIsGrowing = true;

    private void FixedUpdate()
    {
        if (!speedIsGrowing) return;
        
        ApplyEffect(speedBar.CurrentValue);
    }

    [UsedImplicitly] public void SetEffectGrowthActive(bool active) => speedIsGrowing = active;

    private void ApplyEffect(float strength) => effect.color = new Color(1f, 1f, 1f, strength);

    private void OnValidate()
    {
        if(effect == null) return;

        ApplyEffect(startingEffectStrength);
    }
}
