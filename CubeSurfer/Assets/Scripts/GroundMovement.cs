using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    private GameManager _gameManager;
    private ParticleSystem _groundParticles;
    private ParticleSystem.VelocityOverLifetimeModule _groundParticlesVelocity;

    #region EventSubscribers
    void OnEnable()
    {
        StartGameCamera.OnGameStart += StartGroundMovement;
        MoveObstacleToPool.OnLevelup += SpeedUpGroundMovement;
        PlayerController.OnGameOver += StopGroundMovement;
    }
    void OnDisable()
    {
        StartGameCamera.OnGameStart -= StartGroundMovement;
        MoveObstacleToPool.OnLevelup -= SpeedUpGroundMovement;
        PlayerController.OnGameOver -= StopGroundMovement;
    }
    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _groundParticles = GetComponent<ParticleSystem>();
        _groundParticlesVelocity = _groundParticles.velocityOverLifetime;

        //Set particles color based on theme currently selected
        if (CompareTag("GroundParticles"))
        {
            ParticleSystem.MainModule settings = _groundParticles.main;
            Color particlesColor = _gameManager.GroundMaterial.color;
            particlesColor.a = 0.360784f;
            settings.startColor = new ParticleSystem.MinMaxGradient(particlesColor);
        }
    }

    void StartGroundMovement() => _groundParticles.Play();

    void StopGroundMovement() => _groundParticles.Pause();

    void SpeedUpGroundMovement()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        float speed = _gameManager.CurrentSpeed * 1.5f;
        _groundParticlesVelocity.x = new ParticleSystem.MinMaxCurve(speed, curve);
    }
}
