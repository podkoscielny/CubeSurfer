using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacleToPool : MonoBehaviour
{
    public static event Action OnLevelup;

    private ObjectPooler _objectPooler;

    void Start() => _objectPooler = ObjectPooler.Instance;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle")) // Move obstacles to pool, when there is no obstacle left on the map, level up and spawn new wave
        {
            _objectPooler.AddToPool(other.gameObject);

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            if (obstacles.Length == 0) OnLevelup?.Invoke();
        }
    }
}
