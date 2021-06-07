using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left, Mid, Right }

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    public static event Action OnGameOver;

    //Audio
    [SerializeField] AudioClip slideSound;
    [SerializeField] AudioClip jumpSound;
    private AudioSource _playerAudio;
    private const float SOUND_VOLUME = 1f;

    //Player movement
    private Rigidbody _playerRb;
    private const float JUMP_FORCE = 180.0f;
    private const float TORQUE_FORCE = 14.0f;
    private const float SLIDE_FORCE = 38f;

    private const float LANE_BOUND = 6f;
    private float _currentLaneBound = 0f;

    private Side _currentSide = Side.Mid;

    private bool _isOnGround = true;
    private bool _isChangingLane = false;
    private bool _isSlidingLeft = false;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _playerAudio = GetComponent<AudioSource>();
        _playerRb = GetComponent<Rigidbody>();
        _playerRb.interpolation = RigidbodyInterpolation.Interpolate; // prevent player from jittering
    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            ChangeLane();
            Jump();
        }

        if (_isChangingLane) StopHorizontalMovement();
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround && !_isChangingLane && !_gameManager.IsGameOver)
        {
            _playerRb.AddForce(Vector3.up * JUMP_FORCE, ForceMode.Impulse);
            _playerRb.AddTorque(Vector3.right * TORQUE_FORCE, ForceMode.Impulse);
            _isOnGround = false;

            _playerAudio.PlayOneShot(jumpSound, SOUND_VOLUME);
        }
    }

    void ChangeLane()
    {
        if (!_isOnGround || _gameManager.IsGameOver || !_gameManager.HasGameStarted) return;

        if (Input.GetKeyDown(KeyCode.A) && _currentSide != Side.Left)
        {
            MovePlayer(Vector3.left, Side.Left, Side.Right);
        }
        else if (Input.GetKeyDown(KeyCode.D) && _currentSide != Side.Right)
        {
            MovePlayer(Vector3.right, Side.Right, Side.Left);
        }
    }

    void MovePlayer(Vector3 slideDirection, Side farBound, Side nearBound)
    {
        bool isMovingLeft = slideDirection == Vector3.left;

        _playerRb.velocity = new Vector3(0, _playerRb.velocity.y, _playerRb.velocity.z); // Reset velocity on x-axis to smoothly change directions
        _playerRb.AddForce(slideDirection * SLIDE_FORCE, ForceMode.VelocityChange);

        _isChangingLane = true;
        _isSlidingLeft = isMovingLeft;

        //Set current Lane that player is going toward and set position to where player's movement should be stopped 
        if (_currentSide == nearBound)
        {
            _currentSide = Side.Mid;
            _currentLaneBound = 0;
        }
        else
        {
            _currentSide = farBound;
            _currentLaneBound = isMovingLeft ? -LANE_BOUND : LANE_BOUND;
        }

        _playerAudio.PlayOneShot(slideSound, SOUND_VOLUME);
    }

    void StopHorizontalMovement()
    {
        if ((_isSlidingLeft && transform.position.x <= _currentLaneBound) || (!_isSlidingLeft && transform.position.x >= _currentLaneBound)) // Reset velocity when reaches the intended lane
        {
            _playerRb.velocity = new Vector3(0, _playerRb.velocity.y, _playerRb.velocity.z);
            _isChangingLane = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !_gameManager.IsGameOver)
        {
            _isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            OnGameOver?.Invoke();
        }
    }
}
