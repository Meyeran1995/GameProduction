using JetBrains.Annotations;
using UnityEngine;

public class BubbleResourceController : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private Animator sideCharacterAnimator;
    [SerializeField] private AutoFillResourceBar energyBar;

    private void Start() => RegisterWithHandler();

    [UsedImplicitly] public void StartResourceRegeneration() => energyBar.IsReplenishing = true;

    [UsedImplicitly] public void EndResourceRegeneration() => energyBar.IsReplenishing = false;

    public void StartResourceDepletion() => energyBar.IsDepleting = true;

    public void EndResourceDepletion() => energyBar.IsDepleting = false;

    [UsedImplicitly] public void OnLowEnergyDetected() => sideCharacterAnimator.SetBool("EnergyLow", true);

    [UsedImplicitly] public void OnLowEnergyAverted() => sideCharacterAnimator.SetBool("EnergyLow", false);

    #region Restart

    public void Restart() => sideCharacterAnimator.SetBool("EnergyLow", false);

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    #endregion
}
