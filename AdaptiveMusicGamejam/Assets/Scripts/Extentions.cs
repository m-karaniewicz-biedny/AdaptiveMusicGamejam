using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    
    public static Vector3 SmoothStepTo(this Vector3 from, Vector3 to, float t)
    {
        return new Vector3(
            Mathf.SmoothStep(from.x,to.x,t),
            Mathf.SmoothStep(from.y,to.y,t),
            Mathf.SmoothStep(from.z,to.z,t));
    }
    
    
}
