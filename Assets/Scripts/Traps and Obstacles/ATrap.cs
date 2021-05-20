using UnityEngine;

/// <summary>
/// An Trap to be triggered when the player enters its collider
/// </summary>
public abstract class ATrap : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision) => TriggerTrap(collision);

    protected abstract void TriggerTrap(Collider2D collision);
}
