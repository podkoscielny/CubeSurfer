using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCubeController : MonoBehaviour
{
    private Camera mainCamera;

    void Start() => mainCamera = Camera.main;

    void Update() => FollowMousePosition();

    void FollowMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.y));
        transform.LookAt(mousePositionWorld);
    }
}
