using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Renderer cubeRenderer;

    private Theme _currentTheme;
    private Camera _mainCamera;

    void Start()
    {
        _currentTheme = GameManager.Instance.Theme;
        _mainCamera = Camera.main;

        SetMenuTheme();
    }

    private void SetMenuTheme()
    {
        _mainCamera.backgroundColor = _currentTheme.backgroundColor;
        cubeRenderer.material = _currentTheme.playerMaterial;
    }
}
