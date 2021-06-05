using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMultiplier : MonoBehaviour
{
    private const float ROTATION_SPEED = 120f;

    void Update() => RotateMultiplier();

    void RotateMultiplier() => transform.Rotate(Time.deltaTime * Vector3.up * ROTATION_SPEED);
}
