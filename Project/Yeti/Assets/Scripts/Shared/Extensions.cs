using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensions
{
    public static float ReturnObjectOrientation(Transform _transform)
    {
         float objectOrientation = Mathf.Abs(_transform.eulerAngles.z);       
        return objectOrientation;
    }
}
