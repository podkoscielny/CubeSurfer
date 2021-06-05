using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetTransformation(this Transform transform, Transform obj)
    {
        transform.position = obj.position;
        transform.rotation = obj.rotation;
        transform.localScale = obj.localScale;
    }
}
