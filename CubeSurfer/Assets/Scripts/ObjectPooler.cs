using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int poolSize;

    private Queue<GameObject> _obstaclesPool;

    private GameManager _gameManager;
    public static ObjectPooler Instance { get; private set; }

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;

        _obstaclesPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = Instantiate(obstaclePrefab);
            if (_gameManager.PlayerMaterial != null)
                instance.GetComponent<Renderer>().material = _gameManager.PlayerMaterial;

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
