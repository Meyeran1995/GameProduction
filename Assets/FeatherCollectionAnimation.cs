using JetBrains.Annotations;
using UnityEngine;

public class FeatherCollectionAnimation : AListenerEnabler
{
    [SerializeField] private Animator animator;

    [UsedImplicitly]
    public void OnFeatherCollected() => animator.SetTrigger("Collected");
}
