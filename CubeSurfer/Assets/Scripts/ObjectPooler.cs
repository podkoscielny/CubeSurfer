using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int poolSize;

    private Queue<GameObject> _obstaclesPool;

    private Theme _currentTheme;

    void Start()
    {
        _currentTheme = GameManager.Instance.Theme;

        _obstaclesPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = Instantiate(obstaclePrefab);
            instance.GetComponent<Renderer>().material = _currentTheme.obstaclesMaterial;

            AddToPool(instance);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        _obstaclesPool.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        GameObject instance = _obstaclesPool.Dequeue();
        instance.SetActive(true);

        return instance;
    }
}
