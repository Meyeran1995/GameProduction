using UnityEngine;
using RoboRyanTron.Unite2017.Events;

/// <summary>
/// Abstract class with an array of listeners and corresponding OnEnable/Disable functions
/// </summary>
public abstract class AMultiListenerEnabler : MonoBehaviour
{
    [SerializeField] protected GameEventListener[] listeners;

    protected virtual void OnEnable()
    {
        foreach (var listener in listeners)
        {
            listener.OnEnable();
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var listener in listeners)
        {
            listener.OnDisable();
        }
    }
}
