using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicPlatform))]
public class DynamicPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DynamicPlatform dynamicPlatform = (DynamicPlatform)target;

        string rotatingPlatformButtonLabel = null;
        string movingPlatformButtonLabel = null;

        bool hasRotatingPlatform = dynamicPlatform.gameObject.GetComponent<RotatingPlatform>();
        bool hasMovingPlatform = dynamicPlatform.gameObject.GetComponent<MovingPlatform>();

        rotatingPlatformButtonLabel = hasRotatingPlatform? "Remove Rotating Platform": "Add Rotating Platform";
        movingPlatformButtonLabel = hasMovingPlatform? "Remove Moving Platform": "Add Moving Platform";

        if(GUILayout.Button(rotatingPlatformButtonLabel))
        {
            if(hasRotatingPlatform)
                DestroyImmediate(dynamicPlatform.gameObject.GetComponent<RotatingPlatform>());
            else
            {
                dynamicPlatform.gameObject.AddComponent<RotatingPlatform>();
            }
        }

         if(GUILayout.Button(movingPlatformButtonLabel))
        {
            if(hasMovingPlatform)
                DestroyImmediate(dynamicPlatform.gameObject.GetComponent<MovingPlatform>());
            else
            {
                dynamicPlatform.gameObject.AddComponent<MovingPlatform>();
            }
        }
    }
}
