using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left, Mid, Right, MidLeft, MidRight }

public class PlayerController : MonoBehaviour
{
    public static event Action OnGameOver;

    [SerializeField] AudioClip slideSound;
    [SerializeField] AudioClip jumpSound;

    private Rigidbody _playerRb;
    private AudioSource _playerAudio;
    private GameManager _gameManager;
    private float _currentLaneBound = 0f;
    private Side _currentSide = Side.Mid;
    private bool _isOnGround = true;
    private bool _isChangingLane = false;
    private bool _isSlidingLeft = false;

    private const float SOUND_VOLUME = 1f;
    private const float LANE_BOUND = 6f;
    private const float JUMP_FORCE = 180.0f;
    private const float TORQUE_FORCE = 14.0f;
    private const float SLIDE_FORCE = 38f;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _playerAudio = GetComponent<AudioSource>();
        _playerRb = GetComponent<Rigidbody>();
        _playerRb.interpolation = RigidbodyInterpolation.Interpolate; // prevent player from jittering
    }

    void Update()
    {
        if (!_isOnGround || _gameManager.IsGameOver || !_gameManager.HasGameStarted) return;

        if (Time.timeScale > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.A) && _currentSide != Side.Left)
            {
                MovePlayer(Vector3.left, Side.Left, Side.Right);
            }
            else if (Input.GetKeyDown(KeyCode.D) && _currentSide != Side.Right)
            {
                MovePlayer(Vector3.right, Side.Right, Side.Left);
            }
        }

        if (_isChangingLane) StopHorizontalMovement();
    }

    void Jump()
    {
        _playerRb.AddForce(Vector3.up * JUMP_FORCE, ForceMode.Impulse);
        _playerRb.AddTorque(Vector3.right * TORQUE_FORCE, ForceMode.Impulse);
        _isOnGround = false;

        if(_isChangingLane)
        {
            ResetHorizontalVelocity();
            _isChangingLane = false;

            if((_isSlidingLeft && _currentSide == Side.Mid) ||(!_isSlidingLeft && _currentSide == Side.Right))
            {
                _currentSide = Side.MidRight;
            }
            else if((_isSlidingLeft && _currentSide == Side.Left) || (!_isSlidingLeft && _currentSide == Side.Mid))
            {
                _currentSide = Side.MidLeft;
            }
        }

        _playerAudio.PlayOneShot(jumpSound, SOUND_VOLUME);
    }

    void MovePlayer(Vector3 slideDirection, Side farBound, Side nearBound)
    {
        bool isMovingLeft = slideDirection == Vector3.left;

        ResetHorizontalVelocity(); // Reset velocity on x-axis to smoothly change directions
        _playerRb.AddForce(slideDirection * SLIDE_FORCE, ForceMode.VelocityChange);

        _isChangingLane = true;
        _isSlidingLeft = isMovingLeft;

        //Set current Lane that player is going toward and set position to where player's movement should be stopped 
        if (_currentSide == nearBound)
        {
            _currentSide = Side.Mid;
            _currentLaneBound = 0;
        }
        else if(_currentSide == Side.MidLeft)
        {
            _currentSide = isMovingLeft ? Side.Left : Side.Mid;
            _currentLaneBound = isMovingLeft ? -LANE_BOUND : 0;
        }
        else if(_currentSide == Side.MidRight)
        {
            _currentSide = isMovingLeft ? Side.Mid : Side.Right;
            _currentLaneBound = isMovingLeft ? 0 : LANE_BOUND;
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
            ResetHorizontalVelocity();
            _isChangingLane = false;
        }
    }

    void ResetHorizontalVelocity() => _playerRb.velocity = new Vector3(0, _playerRb.velocity.y, _playerRb.velocity.z);

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
