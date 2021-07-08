using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject gameOverLamp;
    [SerializeField] GameObject player;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject boxVolume;
    [SerializeField] GameObject dummySun;
    [SerializeField] GameObject flyingLamps;

    [Header("Cameras")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera startCamera;

    [Header("Lights")]
    [SerializeField] Material lightBulbsMaterial;
    [SerializeField] Material ledStripsMaterial;
    [SerializeField] Light[] lights;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;

    private Color _ambientLight;
    private Material _cloudsMaterial;
    private GameManager _gameManager;
    private Animator _environmentAnimator;
    private AudioSource _environmentAudio;
    private bool _areLightsTurnedOn = false;
    private Color _turnedLightsOffColor = new Color(r: 0, g: 0, b: 0, a: 0);
    private Color _lightBulbsEmissionColor = new Color(r: 1.304f, g: 1.270f, b: 1.086f, a: 1f);
    private Color _ledStripsEmissionColor = new Color(r: 1.662f, g: 1.662f, b: 1.662f, a: 1f);
    private const float TURN_LIGHTS_AT_INTENSITY = 0.55f;
    private const float LIGHTS_INTENSITY = 62f;
    private const float BASE_CLOUDS_INTENSITY = 1.46f;
    private const float BASE_FRESNEL_OPACITY = 1f;
    private const float SWITCH_ON_PITCH = 1.8f;
    private const float SWITCH_OFF_PITCH = 0.8f;

    #region Event Subscribers
    private void OnEnable()
    {
        PlayerController.OnGameOver += SetGameOverProperties;
        StartGameCamera.OnGameStart += EnableFlyingLamps;
    }

    private void OnDisable()
    {
        PlayerController.OnGameOver -= SetGameOverProperties;
        StartGameCamera.OnGameStart -= EnableFlyingLamps;
    }
    #endregion

    void Start()
    {
        InitializeProperties();
        SetThemeColors();
        SetEmissionColor(_turnedLightsOffColor);
    }

    void Update()
    {
        if (_gameManager.HasGameStarted) SetBackgroundColors();
    }

    void InitializeProperties()
    {
        _gameManager = GameManager.Instance;
        _ambientLight = RenderSettings.ambientLight;
        _cloudsMaterial = GameObject.FindGameObjectWithTag("Clouds").GetComponent<Renderer>().material;
        _environmentAnimator = GetComponent<Animator>();
        _environmentAudio = GetComponent<AudioSource>();
    }

    void SetBackgroundColors()
    {
        float intensity = dummySun.transform.position.y;

        mainCamera.backgroundColor = _gameManager.BackgroundColor * intensity;
        RenderSettings.fogColor = _gameManager.FogColor * intensity;
        RenderSettings.ambientLight = _ambientLight * intensity;
        _cloudsMaterial.SetFloat("Vector1_245C3B23", BASE_CLOUDS_INTENSITY * intensity);
        _cloudsMaterial.SetFloat("Vector1_677FFF29", BASE_FRESNEL_OPACITY * intensity);

        if (intensity < TURN_LIGHTS_AT_INTENSITY && !_areLightsTurnedOn)
        {
            //turn the lights on
            _environmentAnimator.SetTrigger("Activate");
            _areLightsTurnedOn = true;
            
        }
        else if (intensity > TURN_LIGHTS_AT_INTENSITY && _areLightsTurnedOn)
        {
            //turn the lights off
            _environmentAnimator.SetTrigger("Deactivate");
            _areLightsTurnedOn = false;
        }
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

            _gameManager.SetTheme(mainCamera.backgroundColor, RenderSettings.fogColor, mainCamera.backgroundColor, groundMaterial, playerMaterial);
        }
    }

    void SetGameOverProperties()
    {
        boxVolume.SetActive(true);
        gameOverLamp.SetActive(true);
        gameOverLamp.transform.LookAt(player.transform);
    }

    void EnableFlyingLamps() => flyingLamps.SetActive(true);

    void TurnLightsOn()
    {
        SetEmissionColor(_lightBulbsEmissionColor, _ledStripsEmissionColor);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = LIGHTS_INTENSITY;
        }

        _environmentAudio.pitch = SWITCH_ON_PITCH;
        _environmentAudio.PlayOneShot(switchOnAudio);
    }

    void TurnLightsOff()
    {
        SetEmissionColor(_turnedLightsOffColor);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0;
        }

        _environmentAudio.pitch = SWITCH_OFF_PITCH;
        _environmentAudio.PlayOneShot(switchOffAudio);
    }

    void SetEmissionColor(Color lampsColor, Color ledsColor)
    {
        lightBulbsMaterial.SetColor("_EmissionColor", lampsColor);
        ledStripsMaterial.SetColor("_EmissionColor", ledsColor);
    }

    void SetEmissionColor(Color color)
    {
        lightBulbsMaterial.SetColor("_EmissionColor", color);
        ledStripsMaterial.SetColor("_EmissionColor", color);
    }
}
