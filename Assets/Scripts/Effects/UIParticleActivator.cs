using UnityEngine;

public class UIParticleActivator : MonoBehaviour
{
    private ParticleSystem[] particleEffects;

    private void Awake() => particleEffects = GetComponentsInChildren<ParticleSystem>();

    private void OnEnable() => ActivateEffects();

    private void OnDisable() => DeactivateEffects();

    public void ActivateEffects()
    {
        foreach (var effect in particleEffects)
        {
            effect.Play();
        }
    }

    public void DeactivateEffects()
    {
        foreach (var effect in particleEffects)
        {
            effect.Stop();
        }
    }
}
