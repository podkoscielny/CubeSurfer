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
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera startCamera;
    [SerializeField] GameObject boxVolume;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;

    private AudioSource _environmentAudio;
    private const float SWITCH_ON_PITCH = 1.4f;
    private const float SWITCH_OFF_PITCH = 0.8f;
    private const float COLOR_CHANGE_SPEED = 0.35f;
    private const float LIGHT_CHANGE_SPEED = 0.18f;
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
        if (_gameManager.PlayerMaterial != null)
        {
            mainCamera.backgroundColor = _gameManager.BackgroundColor;
            startCamera.backgroundColor = _gameManager.BackgroundColor;
            RenderSettings.fogColor = _gameManager.FogColor;
            player.GetComponent<Renderer>().material = _gameManager.PlayerMaterial;
            ground.GetComponent<Renderer>().material = _gameManager.GroundMaterial;
        }
        else
        {
            Material playerMaterial = player.GetComponent<Renderer>().material;
            Material groundMaterial = ground.GetComponent<Renderer>().material;

            _gameManager.SetTheme(mainCamera.backgroundColor, RenderSettings.fogColor, groundMaterial, playerMaterial);
        }
    }

    void SetGlobalProperties(WaveController waveController, float obstacleSpeed)
    {
        // Set global properties based on Wave's unique properties
        StartCoroutine(ChangeLightIntensity(waveController.lightIntensity));
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
        boxVolume.SetActive(true);
        lamps.SetActive(false);
        gameOverLamp.SetActive(true);
        gameOverLamp.transform.LookAt(player.transform);

        if(directionalLight.intensity > GAMEOVER_BACKGROUND_INTENSITY)
            ChangeBackgroundColor(GAMEOVER_BACKGROUND_INTENSITY);
    }

    void ChangeBackgroundColor(float intensityFactor)
    {
        Color desiredColor = _gameManager.BackgroundColor * intensityFactor;
        Color desiredFogColor = _gameManager.FogColor * intensityFactor;
        StartCoroutine(ChangeColor(desiredColor, desiredFogColor));
    }

    IEnumerator ChangeColor(Color desiredColor, Color desiredFogColor)
    {
        float tick = 0f;
        Color currentFogColor = RenderSettings.fogColor;

        while (mainCamera.backgroundColor != desiredColor)
        {
            tick += Time.unscaledDeltaTime * COLOR_CHANGE_SPEED;
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, desiredColor, tick);
            RenderSettings.fogColor = Color.Lerp(currentFogColor, desiredFogColor, tick);
            yield return null;
        }
    }

    IEnumerator ChangeLightIntensity(float desiredIntensity)
    {
        float tick = 0f;

        while (directionalLight.intensity != desiredIntensity)
        {
            tick += Time.unscaledDeltaTime * LIGHT_CHANGE_SPEED;
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, desiredIntensity, tick);
            yield return null;
        }
    }
}
