using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] Renderer cubeRenderer;

    void Start()
    {
        _gameManager = GameManager.Instance;

        if(_gameManager.SkyboxMaterial != null)
        {
            RenderSettings.skybox = _gameManager.SkyboxMaterial;
            cubeRenderer.material = _gameManager.PlayerMaterial;
        }
    }
}
