using UnityEngine;

public class UIParticleActivator : AMultiListenerEnabler
{
    private ParticleSystem[] particleEffects;

    private void Awake()
    {
        particleEffects = GetComponentsInChildren<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ActivateEffects();
    }

    public void ActivateEffects()
    {
        foreach (var effect in particleEffects)
        {
            effect.Play();
        }
    }
}
