using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public float speed;
    public bool areLightsTurnedOn;
    public bool hasMultiplier;
    public Vector3 multipliersPositon;
    [Range(0f,1f)] public float lightIntensity;
    [Range(0f,1f)] public float backgroundColorIntensity;
}
