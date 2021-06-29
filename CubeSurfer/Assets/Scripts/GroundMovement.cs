using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    private GameManager _gameManager;
    private ParticleSystem _groundParticles;

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
    }

    void StartGroundMovement() => _groundParticles.Play();

    void StopGroundMovement() => _groundParticles.Pause();

    void SpeedUpGroundMovement()
    {
        ParticleSystem.VelocityOverLifetimeModule velocity = _groundParticles.velocityOverLifetime;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        float speed = _gameManager.CurrentSpeed * 1.5f;
        velocity.x = new ParticleSystem.MinMaxCurve(speed, curve);
    }
}
