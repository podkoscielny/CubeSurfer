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
    [SerializeField] GameObject dummySun;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;

    private AudioSource _environmentAudio;
    private const float SWITCH_ON_PITCH = 1.4f;
    private const float SWITCH_OFF_PITCH = 0.8f;
    private const float COLOR_CHANGE_SPEED = 0.35f;
    private const float LIGHT_CHANGE_SPEED = 0.18f;
    private const float GAMEOVER_BACKGROUND_INTENSITY = 0.4f;
    private const float INTENSITY_ADDITION = 0.500352f;

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

    void Update()
    {
        if (_gameManager.HasGameStarted) SetBackgroundColors();
    }

    void SetBackgroundColors()
    {
        float intensity = dummySun.transform.position.y + INTENSITY_ADDITION;

        mainCamera.backgroundColor = _gameManager.BackgroundColor * intensity;
        RenderSettings.fogColor = _gameManager.FogColor * intensity;
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

        //Color desiredBackgroundColor = _gameManager.BackgroundColor * waveController.backgroundColorIntensity;
        //Color desiredFogColor = _gameManager.FogColor * waveController.backgroundColorIntensity;

        //StartCoroutine(ChangeColor(desiredBackgroundColor));
        //StartCoroutine(ChangeFogIntensity(desiredFogColor));
        //StartCoroutine(ChangeLightIntensity(waveController.lightIntensity));
    }

    void SetGameOverProperties()
    {
        boxVolume.SetActive(true);
        lamps.SetActive(false);
        gameOverLamp.SetActive(true);
        gameOverLamp.transform.LookAt(player.transform);

        if(directionalLight.intensity > GAMEOVER_BACKGROUND_INTENSITY)
        {
            Color desiredColor = _gameManager.BackgroundColor * GAMEOVER_BACKGROUND_INTENSITY;
            StartCoroutine(ChangeColor(desiredColor));
        }
    }

    IEnumerator ChangeColor(Color desiredColor)
    {
        float tick = 0f;

        while (mainCamera.backgroundColor != desiredColor)
        {
            tick += Time.unscaledDeltaTime * COLOR_CHANGE_SPEED;
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, desiredColor, tick);
            yield return null;
        }
    }

    IEnumerator ChangeFogIntensity(Color desiredColor)
    {
        float tick = 0f;

        while (RenderSettings.fogColor != desiredColor)
        {
            tick += Time.unscaledDeltaTime * COLOR_CHANGE_SPEED;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, desiredColor, tick);
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
