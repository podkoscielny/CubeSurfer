using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMovement : MonoBehaviour
{
    private GameManager _gameManager;
    private ParticleSystem _particles;
    private ParticleSystem.VelocityOverLifetimeModule _particlesVelocity;
    private AnimationCurve _curve;

    #region EventSubscribers
    void OnEnable()
    {
        StartGameCamera.OnGameStart += StartParticleMovement;
        MoveObstacleToPool.OnLevelup += SpeedUpParticleMovement;
        PlayerController.OnGameOver += StopParticleMovement;
    }
    void OnDisable()
    {
        StartGameCamera.OnGameStart -= StartParticleMovement;
        MoveObstacleToPool.OnLevelup -= SpeedUpParticleMovement;
        PlayerController.OnGameOver -= StopParticleMovement;
    }
    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _particles = GetComponent<ParticleSystem>();
        _particlesVelocity = _particles.velocityOverLifetime;

        InitializeAnimationCurve();

        if (CompareTag("GroundParticles")) SetParticlesColor();  //Set particles color based on theme currently selected
    }

    void StartParticleMovement() => _particles.Play();

    void StopParticleMovement() => _particles.Pause();

    void SpeedUpParticleMovement()
    {
        float speed = _gameManager.CurrentSpeed * 1.5f;
        _particlesVelocity.x = new ParticleSystem.MinMaxCurve(speed, _curve);
    }

    void InitializeAnimationCurve()
    {
        _curve = new AnimationCurve();
        _curve.AddKey(0.0f, 1.0f);
        _curve.AddKey(1.0f, 0.0f);
    }

    void SetParticlesColor()
    {
        ParticleSystem.MainModule settings = _particles.main;
        Color particlesColor = _gameManager.Theme.groundMaterial.color;
        particlesColor.a = 0.360784f;
        settings.startColor = new ParticleSystem.MinMaxGradient(particlesColor);
    }
}
