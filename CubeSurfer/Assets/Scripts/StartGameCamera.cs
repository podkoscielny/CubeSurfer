using System;
using UnityEngine;

public class StartGameCamera : MonoBehaviour
{
    public static event Action OnGameStart;

    [SerializeField] GameObject mainCamera;

    void SetMainCamera()
    {
        mainCamera.SetActive(true);
        gameObject.SetActive(false);
    }

    void StartGame() => OnGameStart?.Invoke();
}
