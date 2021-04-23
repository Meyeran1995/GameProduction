// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------


// modified version

using UnityEngine;
using UnityEngine.Events;

namespace RoboRyanTron.Unite2017.Events
{
    [System.Serializable]
    public class GameEventListener
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        public void OnEnable()
        {
            Event?.RegisterListener(this);
        }

        public void OnDisable()
        {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response?.Invoke();
        }
    }
}