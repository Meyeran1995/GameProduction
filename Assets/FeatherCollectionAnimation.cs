using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class FeatherCollectionAnimation : AListenerEnabler
{
    [SerializeField] private Animator animator;

    [UsedImplicitly]
    public void OnFeatherCollected() => animator.SetTrigger("Collected");
}
