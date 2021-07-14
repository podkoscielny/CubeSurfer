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
    [SerializeField] GameObject lights;

    [Header("Cameras")]
    [SerializeField] Camera mainCamera;

    private Theme _currentTheme;
    private Color _ambientLight;
    private GameManager _gameManager;
    private Animator _environmentAnimator;
    private Material _cloudsMaterial;
    private bool _areLightsTurnedOn = false;
    private const float TURN_LIGHTS_AT_INTENSITY = 0.55f;
    private const float BASE_CLOUDS_INTENSITY = 1.46f;
    private const float BASE_FRESNEL_OPACITY = 1f;

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
        _cloudsMaterial = GameObject.FindGameObjectWithTag("Clouds").GetComponent<Renderer>().material;
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

    void SwitchLights() => lights.SetActive(_areLightsTurnedOn); //Invoke on Animation Event
}
