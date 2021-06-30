using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Renderer cubeRenderer;

    private GameManager _gameManager;
    private Camera _mainCamera;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _mainCamera = Camera.main;

        SetMenuTheme();
    }

    private void SetMenuTheme()
    {
        if (_gameManager.PlayerMaterial != null)
        {
            _mainCamera.backgroundColor = _gameManager.MainMenuBackgroundColor;
            cubeRenderer.material = _gameManager.PlayerMaterial;
        }
    }
}
