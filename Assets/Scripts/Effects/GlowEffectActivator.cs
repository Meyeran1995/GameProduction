using JetBrains.Annotations;
using UnityEngine;

public class GlowEffectActivator : UIEffectActivator
{
    [SerializeField] private CircularResourceBar speedBar;
    private bool speedIsGrowing = true;

    private void FixedUpdate()
    {
        if (!speedIsGrowing) return;
        
        ApplyEffect(speedBar.CurrentValue);
    }

    [UsedImplicitly] public void SetEffectGrowthActive(bool active) => speedIsGrowing = active;
}
