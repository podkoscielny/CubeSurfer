using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject gameOverLamp;
    [SerializeField] GameObject player;
    [SerializeField] GameObject boxVolume;
    [SerializeField] GameObject dummySun;
    [SerializeField] GameObject flyingLamps;

    [Header("Cameras")]
    [SerializeField] Camera mainCamera;

    [Header("Lights")]
    [SerializeField] Material lightBulbsMaterial;
    [SerializeField] Material ledStripsMaterial;
    [SerializeField] Light[] lights;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;

    private Theme _currentTheme;
    private Color _ambientLight;
    private GameManager _gameManager;
    private Animator _environmentAnimator;
    private AudioSource _environmentAudio;
    private Material _cloudsMaterial;
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
        _gameManager = GameManager.Instance;
        _currentTheme = _gameManager.Theme;
        _ambientLight = RenderSettings.ambientLight;
        _environmentAnimator = GetComponent<Animator>();
        _environmentAudio = GetComponent<AudioSource>();
        _cloudsMaterial = GameObject.FindGameObjectWithTag("Clouds").GetComponent<Renderer>().material;

        lightBulbsMaterial.SetColor("_EmissionColor", _turnedLightsOffColor);
        ledStripsMaterial.SetColor("_EmissionColor", _turnedLightsOffColor);
    }

    void Update()
    {
        if (_gameManager.HasGameStarted) SetBackgroundColors();
    }

    void SetBackgroundColors()
    {
        float intensity = dummySun.transform.position.y;

        mainCamera.backgroundColor = _currentTheme.backgroundColor * intensity;
        RenderSettings.fogColor = _currentTheme.fogColor * intensity;
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

    void SetGameOverProperties()
    {
        boxVolume.SetActive(true);
        gameOverLamp.SetActive(true);
        gameOverLamp.transform.LookAt(player.transform);
    }

    void EnableFlyingLamps() => flyingLamps.SetActive(true);

    void TurnLightsOn() => SwitchLights(_lightBulbsEmissionColor, _ledStripsEmissionColor, LIGHTS_INTENSITY, SWITCH_ON_PITCH, switchOnAudio);

    void TurnLightsOff() => SwitchLights(_turnedLightsOffColor, _turnedLightsOffColor, 0, SWITCH_OFF_PITCH, switchOffAudio);

    void SwitchLights(Color lampsColor, Color ledsColor, float lightsIntensity, float pitch, AudioClip switchingAudio)
    {
        lightBulbsMaterial.SetColor("_EmissionColor", lampsColor);
        ledStripsMaterial.SetColor("_EmissionColor", ledsColor);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = lightsIntensity;
        }

        _environmentAudio.pitch = pitch;
        _environmentAudio.PlayOneShot(switchingAudio);
    }
}
