using System;
using UnityEngine;

public class StartGameCamera : MonoBehaviour
{
    public static event Action OnGameStart;

    [SerializeField] GameObject mainCamera;
    [SerializeField] RotateTheSun mainLightScript; 

    void SetMainCamera()
    {
        mainLightScript.StartRotating();
        mainCamera.SetActive(true);
        gameObject.SetActive(false);
    }

    void StartGame() => OnGameStart?.Invoke();
}
