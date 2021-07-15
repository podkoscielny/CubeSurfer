using System;
using UnityEngine;

public class StartGameCamera : MonoBehaviour
{
    public static event Action OnGameStart;

    [SerializeField] GameObject mainCamera;
    [SerializeField] RotateTheSun mainLightScript; 

    void SetMainCamera() //Invoke on Animation Event
    {
        mainLightScript.enabled = true;
        mainCamera.SetActive(true);
        gameObject.SetActive(false);
    }

    void StartGame() => OnGameStart?.Invoke(); //Invoke on Animation Event
}
