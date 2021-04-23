using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainCharacterMovement))]
public class UnstuckTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MainCharacterMovement o = target as MainCharacterMovement;
        if (GUILayout.Button("Unstuck"))
            o.UnStuckCharacter();
    }
}