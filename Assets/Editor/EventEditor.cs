// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

// Modified to only be active when there are actually active listeners for the event

using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.Unite2017.Events
{
    [CustomEditor(typeof(GameEvent))]
    public class EventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GameEvent gameEvent = target as GameEvent;

            GUI.enabled = Application.isPlaying && gameEvent.ListenerCount > 0;

            if (GUILayout.Button("Raise"))
                gameEvent.Raise();
        }
    }
}