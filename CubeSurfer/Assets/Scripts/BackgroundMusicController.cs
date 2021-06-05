using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioSource _backgroundAudio;
    private const float TURN_VOLUME_DOWN_SPEED = 10f;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _backgroundAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_gameManager.IsGameOver && _backgroundAudio.volume > 0)
            TurnVolumeDown();
    }

    void TurnVolumeDown() =>
        _backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, 0f, TURN_VOLUME_DOWN_SPEED * Time.deltaTime);
}
