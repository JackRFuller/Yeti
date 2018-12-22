using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LerpingClasses
{
    [System.Serializable]
   public class FloatLerp
   {
       public float lerpSpeed;       
       [HideInInspector] public float startValue;
       [HideInInspector] public float targetValue;
       [HideInInspector] public bool hasStartedLerp;
       [HideInInspector] public float timeStartedLerping;

       public float ReturnPercentageComplete()
       {
           float timeSinceStarted = Time.time - timeStartedLerping;
           float percenatgeComplete = timeSinceStarted / lerpSpeed;
           return percenatgeComplete;
       }
   }

}
