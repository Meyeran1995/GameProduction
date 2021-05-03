using UnityEngine;

public abstract class ATrap : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision) => TriggerTrap(collision);

    protected abstract void TriggerTrap(Collider2D collision);
}
