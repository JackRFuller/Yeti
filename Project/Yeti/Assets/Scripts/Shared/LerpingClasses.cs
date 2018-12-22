using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LerpingClasses
{
    public float lerpSpeed;      
    [HideInInspector] public bool hasStartedLerp;
    [HideInInspector] public float timeStartedLerping;

    public float ReturnPercentageComplete()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percenatgeComplete = timeSinceStarted / lerpSpeed;
        return percenatgeComplete;
    }
}

[System.Serializable]
public class Vector3Lerp : LerpingClasses
{   
    [HideInInspector] public Vector3 startValue;
    [HideInInspector] public Vector3 targetValue;

    public Vector3 ReturnLerpProgress(float percentageComplete)
    {
        return Vector3.Lerp(startValue, targetValue, percentageComplete);
    }
}

[System.Serializable]
public class FloatLerp : LerpingClasses
{ 
    [HideInInspector] public float startValue;
    [HideInInspector] public float targetValue;

    public float ReturnLerpProgress(float percentageComplete)
    {
        return Mathf.Lerp(startValue, targetValue, percentageComplete);
    }
}

