using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Renderer cubeRenderer;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;

        SetMenuTheme();
        if (Time.timeScale != 1) SetTimescale();
    }

    private void SetMenuTheme()
    {
        if (_gameManager.SkyboxMaterial != null)
        {
            RenderSettings.skybox = _gameManager.SkyboxMaterial;
            cubeRenderer.material = _gameManager.PlayerMaterial;
        }
    }

    private void SetTimescale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
