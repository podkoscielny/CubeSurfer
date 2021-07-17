using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject pointMultiplier;
    [SerializeField] GameObject[] wavePrefabs;

    private GameManager _gameManager;
    private ObjectPooler _objectPooler;
    [SerializeField] int _currentWaveIndex = 0;
    private float _speedMultiplier = 1f;
    private const float INCREASE_MULTIPLIER_BY = 0.5f;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _objectPooler = ObjectPooler.Instance;
    }

    #region Event Subscribers

    private void OnEnable()
    {
        MoveObstacleToPool.OnLevelup += SpawnWave;
        StartGameCamera.OnGameStart += SpawnWave;
    }

    private void OnDisable()
    {
        MoveObstacleToPool.OnLevelup -= SpawnWave;
        StartGameCamera.OnGameStart -= SpawnWave;
    }

    #endregion

    private void SpawnWave()
    {
        GameObject waveToSpawn = wavePrefabs[_currentWaveIndex];
        WaveController waveController = waveToSpawn.GetComponent<WaveController>();

        _gameManager.CurrentSpeed = waveController.speed * _speedMultiplier;

        InstantiateObstacles(waveToSpawn);
        if (waveController.hasMultiplier)
            SpawnMultiplier(waveController.multipliersPositon);

        _currentWaveIndex++;

        if (_currentWaveIndex == wavePrefabs.Length)
        {
            _currentWaveIndex = 0;
            _speedMultiplier += INCREASE_MULTIPLIER_BY;
        }
    }

    private void InstantiateObstacles(GameObject waveToSpawn)
    {
        // Get obstacles from pool and initialize their transform so they match transform from wavePrefab
        foreach (Transform child in waveToSpawn.transform)
        {
            GameObject obstacle = _objectPooler.GetFromPool();
            obstacle.transform.SetTransformation(child);

            MovingObstacle childScript = child.gameObject.GetComponent<MovingObstacle>();

            if (childScript != null && childScript.enabled)
            {
                MovingObstacle obstacleScript = obstacle.GetComponent<MovingObstacle>();
                obstacleScript.enabled = true;
                obstacleScript.SetProperties(childScript);
            }
        }
    }

    private void SpawnMultiplier(Vector3 position)
    {
        pointMultiplier.SetActive(true);

        pointMultiplier.transform.position = position;
    }
}
