using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTheSun : MonoBehaviour
{
    [SerializeField] GameObject dummySun;

    private GameManager _gameManager;
    private const float ROTATION_SPEED = 10f;

    void Start() => _gameManager = GameManager.Instance;

    void Update()
    {
        if (_gameManager.HasGameStarted) transform.Rotate(Vector3.right, Time.deltaTime * ROTATION_SPEED);
        dummySun.transform.rotation = transform.rotation;
    }
}
