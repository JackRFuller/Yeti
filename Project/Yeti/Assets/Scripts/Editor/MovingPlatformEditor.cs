using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovingPlatform movingPlatform = (MovingPlatform)target;

        if(GUILayout.Button("Set Starting Position"))
        {
            movingPlatform.SetStartingPosition();
        }

        if(GUILayout.Button("Set Target Position"))
        {
            movingPlatform.SetTargetPosition();
        }

        if(GUILayout.Button("Set to Starting Position"))
        {
            movingPlatform.SetToStartingPosition();
        }
    }
}
