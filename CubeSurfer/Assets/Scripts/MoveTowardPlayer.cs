using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    private GameManager _gameManager;

    void Start() => _gameManager = GameManager.Instance;

    void Update()
    {
        if (!_gameManager.IsGameOver) MoveObject();
    }

    void MoveObject() => transform.position += Vector3.back * Time.deltaTime * _gameManager.CurrentSpeed;
}
