using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static event Action<WaveController, float> OnEnvironmentChange;

    private ObjectPooler _objectPooler;
    private GameManager _gameManager;

    [SerializeField] GameObject pointMultiplier;
    private MoveTowardPlayer _multiplierScript;

    [SerializeField] GameObject[] wavePrefabs;
    private int _currentWaveIndex = 0;

    private float _speedMultiplier = 1f;
    private const float SPAWN_DELAY = 3f;
    private const float INCREASE_MULTIPLIER_BY = 0.5f;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        _gameManager = GameManager.Instance;

        _multiplierScript = pointMultiplier.GetComponent<MoveTowardPlayer>();

        SpawnWave();
    }

    #region Event Subscribers

    private void OnEnable()
    {
        MoveObstacleToPool.OnLevelup += SpawnWave;
    }

    private void OnDisable()
    {
        MoveObstacleToPool.OnLevelup -= SpawnWave;
    }

    #endregion

    private void SpawnWave() => StartCoroutine(SetWave());

    IEnumerator SetWave()
    {
        GameObject waveToSpawn = wavePrefabs[_currentWaveIndex];
        WaveController waveController = waveToSpawn.GetComponent<WaveController>();
        float obstacleSpeed = waveController.speed * _speedMultiplier;

        OnEnvironmentChange?.Invoke(waveController, obstacleSpeed);

        yield return new WaitForSeconds(SPAWN_DELAY);

        InstantiateObstacles(waveToSpawn, obstacleSpeed);
        if (waveController.hasMultiplier)
            SpawnMultiplier(waveController.multipliersPositon, obstacleSpeed);

        _currentWaveIndex++;

        if (_currentWaveIndex == wavePrefabs.Length)
        {
            _currentWaveIndex = 0;
            _speedMultiplier += INCREASE_MULTIPLIER_BY;
        }

        if (!_gameManager.HasGameStarted) _gameManager.HasGameStarted = true;
    }

    private void InstantiateObstacles(GameObject waveToSpawn, float obstacleSpeed)
    {
        // Get obstacles from pool and initialize their transform so they match transform from wavePrefab
        foreach (Transform child in waveToSpawn.transform)
        {
            GameObject obstacle = _objectPooler.GetFromPool();
            MoveTowardPlayer obstacleScript = obstacle.GetComponent<MoveTowardPlayer>();

            obstacleScript.speed = obstacleSpeed; // Set obstacle speed to speed declared in wavePrefab
            obstacle.transform.SetTransformation(child);
        }
    }

    private void SpawnMultiplier(Vector3 position, float speed)
    {
        pointMultiplier.SetActive(true);

        _multiplierScript.speed = speed;
        pointMultiplier.transform.position = position;
    }
}
