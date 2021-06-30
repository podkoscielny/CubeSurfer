using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Environment Change")]
    [SerializeField] GameObject gameOverLamp;
    [SerializeField] GameObject player;
    [SerializeField] GameObject ground;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera startCamera;
    [SerializeField] GameObject boxVolume;
    [SerializeField] GameObject dummySun;
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
    private const float TURN_LIGHTS_AT_INTENSITY = 0.55f;
    private const float LIGHTS_INTENSITY = 62f;
    private const float BASE_CLOUDS_INTENSITY = 1.46f;
    private const float SWITCH_ON_PITCH = 1.8f;
    private const float SWITCH_OFF_PITCH = 0.8f;
    private const float INTENSITY_ADDITION = 0.500352f;

    #region Event Subscribers
    private void OnEnable()
    {
        PlayerController.OnGameOver += SetGameOverProperties;
    }

    private void OnDisable()
    {
        PlayerController.OnGameOver -= SetGameOverProperties;
        lightBulbsMaterial.DisableKeyword("_EMISSION");
        ledStripsMaterial.DisableKeyword("_EMISSION");
    }
    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _ambientLight = RenderSettings.ambientLight;
        _cloudsMaterial = GameObject.FindGameObjectWithTag("Clouds").GetComponent<Renderer>().material;
        _environmentAnimator = GetComponent<Animator>();
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
        RenderSettings.ambientLight = _ambientLight * intensity;
        _cloudsMaterial.SetFloat("Vector1_245C3B23", BASE_CLOUDS_INTENSITY * intensity);

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

    void TurnLightsOn()
    {
        lightBulbsMaterial.EnableKeyword("_EMISSION");
        ledStripsMaterial.EnableKeyword("_EMISSION");

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = LIGHTS_INTENSITY;
        }

        _environmentAudio.pitch = SWITCH_ON_PITCH;
        _environmentAudio.PlayOneShot(switchOnAudio);
    }

    void TurnLightsOff()
    {
        lightBulbsMaterial.DisableKeyword("_EMISSION");
        ledStripsMaterial.DisableKeyword("_EMISSION");

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0;
        }

        _environmentAudio.pitch = SWITCH_OFF_PITCH;
        _environmentAudio.PlayOneShot(switchOffAudio);
    }
}
