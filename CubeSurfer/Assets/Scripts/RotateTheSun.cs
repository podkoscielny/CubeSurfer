using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTheSun : MonoBehaviour
{
    [SerializeField] GameObject dummySun;

    private bool _isMainCameraSet = false;
    private const float ROTATION_SPEED = 10f;

    void Update()
    {
        if (_isMainCameraSet) transform.Rotate(Vector3.right, Time.deltaTime * ROTATION_SPEED);
        dummySun.transform.rotation = transform.rotation;
    }

    public void StartRotating() => _isMainCameraSet = true;
}
