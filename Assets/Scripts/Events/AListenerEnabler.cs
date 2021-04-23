using UnityEngine;
using RoboRyanTron.Unite2017.Events;

/// <summary>
/// Abstract class with a listener and corresponding OnEnable/Disable functions
/// </summary>
public abstract class AListenerEnabler : MonoBehaviour
{
    [SerializeField] protected GameEventListener listener;

    public virtual void OnEnable()
    {
        listener.OnEnable();
    }

    public virtual void OnDisable()
    {
        listener.OnDisable();
    }
}
