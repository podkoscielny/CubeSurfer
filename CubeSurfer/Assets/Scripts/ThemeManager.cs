using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera startCamera;
    [SerializeField] Renderer playerRenderer;
    [SerializeField] Renderer groundRenderer;
    [SerializeField] Renderer multiplierRenderer;
    [SerializeField] Renderer cloudsRenderer;
    [SerializeField] ParticleSystem multiplierParticles;

    private Theme _currentTheme;

    void Start()
    {
        _currentTheme = GameManager.Instance.Theme;

        SetThemeColors();
    }

    void SetThemeColors()
    {
        mainCamera.backgroundColor = _currentTheme.backgroundColor;
        startCamera.backgroundColor = _currentTheme.backgroundColor;
        RenderSettings.fogColor = _currentTheme.fogColor;
        playerRenderer.material = _currentTheme.playerMaterial;
        groundRenderer.material = _currentTheme.groundMaterial;
        multiplierRenderer.material = _currentTheme.multiplierMaterial;
        cloudsRenderer.material = _currentTheme.cloudsMaterial;

        ParticleSystem.MainModule settings = multiplierParticles.main;
        settings.startColor = _currentTheme.multiplierMaterial.color;
    }
}
