using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Environment Change")]
    [SerializeField] GameObject lamps;
    [SerializeField] GameObject gameOverLamp;
    [SerializeField] Light directionalLight;
    [SerializeField] GameObject player;
    [SerializeField] GameObject ground;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;

    private AudioSource _environmentAudio;
    private const float SWITCH_ON_PITCH = 1.4f;
    private const float SWITCH_OFF_PITCH = 0.8f;
    private const float COLOR_CHANGE_SPEED = 2f;
    private const float GAMEOVER_BACKGROUND_INTENSITY = 0.4f;

    #region Event Subscribers
    private void OnEnable()
    {
        PlayerController.OnGameOver += SetGameOverProperties;
        SpawnManager.OnEnvironmentChange += SetGlobalProperties;
    }

    private void OnDisable()
    {
        PlayerController.OnGameOver -= SetGameOverProperties;
        SpawnManager.OnEnvironmentChange -= SetGlobalProperties;
    }
    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _environmentAudio = GetComponent<AudioSource>();

        SetThemeColors();
    }

    void SetThemeColors()
    {
        if (_gameManager.SkyboxMaterial != null)
        {
            RenderSettings.skybox = _gameManager.SkyboxMaterial;
            RenderSettings.fogColor = _gameManager.FogColor;
            RenderSettings.ambientGroundColor = _gameManager.LightingGradient;
            player.GetComponent<Renderer>().material = _gameManager.PlayerMaterial;
            ground.GetComponent<Renderer>().material = _gameManager.GroundMaterial;
        }
        else
        {
            Material playerMaterial = player.GetComponent<Renderer>().material;
            Material groundMaterial = ground.GetComponent<Renderer>().material;

            _gameManager.SetTheme(RenderSettings.skybox, RenderSettings.fogColor, RenderSettings.ambientGroundColor, groundMaterial, playerMaterial);
        }
    }

    void SetGlobalProperties(WaveController waveController, float obstacleSpeed)
    {
        // Set global properties based on Wave's unique properties
        directionalLight.intensity = waveController.lightIntensity;
        _gameManager.CurrentSpeed = obstacleSpeed;

        if (!waveController.areLightsTurnedOn && lamps.activeInHierarchy)
        {
            _environmentAudio.pitch = SWITCH_OFF_PITCH;
            _environmentAudio.PlayOneShot(switchOffAudio);
        }
        else if (waveController.areLightsTurnedOn && !lamps.activeInHierarchy)
        {
            _environmentAudio.pitch = SWITCH_ON_PITCH;
            _environmentAudio.PlayOneShot(switchOnAudio);
        }

        lamps.SetActive(waveController.areLightsTurnedOn);

        ChangeBackgroundColor(waveController.backgroundColorIntensity);
    }

    void SetGameOverProperties()
    {
        lamps.SetActive(false);
        gameOverLamp.SetActive(true);
        gameOverLamp.transform.LookAt(player.transform);

        if(directionalLight.intensity > GAMEOVER_BACKGROUND_INTENSITY)
            ChangeBackgroundColor(GAMEOVER_BACKGROUND_INTENSITY);
    }

    void ChangeBackgroundColor(float intensityFactor)
    {
        Color desiredTopColor = _gameManager.SkyboxTopColor * intensityFactor;
        Color desiredBottomColor = _gameManager.SkyboxBottomColor * intensityFactor;
        Color desiredFogColor = _gameManager.FogColor * intensityFactor;
        StartCoroutine(ChangeColor(desiredTopColor, desiredBottomColor, desiredFogColor));
    }

    IEnumerator ChangeColor(Color desiredTopColor, Color desiredBottomColor, Color desiredFogColor)
    {
        float tick = 0f;
        Color currentTopColor = _gameManager.SkyboxMaterial.GetColor("_SkyGradientTop");
        Color currentBottomColor = _gameManager.SkyboxMaterial.GetColor("_SkyGradientBottom");
        Color currentFogColor = RenderSettings.fogColor;

        while (_gameManager.SkyboxMaterial.GetColor("_SkyGradientTop") != desiredTopColor && _gameManager.SkyboxMaterial.GetColor("_SkyGradientBottom") != desiredBottomColor)
        {
            tick += Time.deltaTime * COLOR_CHANGE_SPEED;
            _gameManager.SkyboxMaterial.SetColor("_SkyGradientTop", Color.Lerp(currentTopColor, desiredTopColor, tick));
            _gameManager.SkyboxMaterial.SetColor("_SkyGradientBottom", Color.Lerp(currentBottomColor, desiredBottomColor, tick));
            RenderSettings.fogColor = Color.Lerp(currentFogColor, desiredFogColor, tick);
            yield return null;
        }
    }
}
