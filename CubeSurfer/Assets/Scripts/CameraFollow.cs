using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform fog;

    private GameManager _gameManager;

    private Vector3 _gameplayOffset = new Vector3(0, 4.18f, -9.7f);
    private Vector3 _gameoverOffset = new Vector3(0, 2f, -5f);
    private Vector3 _velocity = Vector3.zero; // ref value for SmoothDamp
    private const float SMOOTH_TIME = 0.125f;

    void Start() => _gameManager = GameManager.Instance;

    void LateUpdate() => FollowPlayersPosition();

    void FollowPlayersPosition()
    {
        _gameplayOffset.x = player.position.x;
        Vector3 desiredPosition = _gameplayOffset;

        if (_gameManager.IsGameOver) desiredPosition = player.position + _gameoverOffset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, SMOOTH_TIME);
        transform.LookAt(fog);
    }
}
