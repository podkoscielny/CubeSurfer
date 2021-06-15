using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ThemeColor
{
    public string name;
    public Color fogColor;
    public Color backgroundColor;
    public Material playerMaterial;
    public Material groundMaterial;
}

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Renderer cubeRenderer;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] TMP_Dropdown themeDropdown;
    [SerializeField] List<ThemeColor> themes;

    private GameManager _gameManager;
    private Resolution[] _resolutions;
    private Camera _mainCamera;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _mainCamera = Camera.main;

        SetQualityOptions();
        SetThemeOptions();
        SetResolutionOptions();
    }

    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTheme(int themeIndex)
    {
        ThemeColor theme = themes[themeIndex];

        _mainCamera.backgroundColor = theme.backgroundColor;
        cubeRenderer.material = theme.playerMaterial;

        _gameManager.SetTheme(theme.backgroundColor, theme.fogColor, theme.groundMaterial, theme.playerMaterial);
    }

    private void SetQualityOptions()
    {
        graphicsDropdown.ClearOptions();

        List<string> qualityOptions = new List<string>();
        string[] qualityNames = QualitySettings.names;

        foreach (string name in qualityNames)
        {
            qualityOptions.Add(name);
        }

        graphicsDropdown.AddOptions(qualityOptions);
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();
    }

    private void SetThemeOptions()
    {
        themeDropdown.ClearOptions();

        List<string> themeOptions = new List<string>();

        int currentThemeIndex = 0;

        for (int i = 0; i < themes.Count; i++)
        {
            string themeName = themes[i].name;
            themeOptions.Add(themeName);

            if (themes[i].backgroundColor == _gameManager.BackgroundColor)
                currentThemeIndex = i;
        }

        themeDropdown.AddOptions(themeOptions);
        themeDropdown.value = currentThemeIndex;
        themeDropdown.RefreshShownValue();
    }

    private void SetResolutionOptions()
    {
        _resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = $"{_resolutions[i].width} x {_resolutions[i].height}";
            resolutionOptions.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
