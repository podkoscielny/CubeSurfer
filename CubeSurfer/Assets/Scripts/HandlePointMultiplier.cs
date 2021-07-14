using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePointMultiplier : MonoBehaviour
{
    [SerializeField] AudioClip pickedMultiplierSound;
    [SerializeField] GameObject pointMultiplierText;
    [SerializeField] GameObject multiplierIndicator;

    private AudioSource _playerAudio;
    private GameManager _gameManager;
    private bool _isMultiplierActive = false;

    private const float SOUND_VOLUME = 1f;
    private const float MULTIPLIER_DURATION = 10f;

    void Start()
    {
        _playerAudio = GetComponent<AudioSource>();
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PointMultiplier"))
        {
            SwitchMultiplier();
            _playerAudio.PlayOneShot(pickedMultiplierSound, SOUND_VOLUME);
            other.gameObject.SetActive(false);
            Invoke(nameof(SwitchMultiplier), MULTIPLIER_DURATION);
        }
    }

    void SwitchMultiplier()
    {
        _isMultiplierActive = !_isMultiplierActive;
        pointMultiplierText.SetActive(_isMultiplierActive);
        multiplierIndicator.SetActive(_isMultiplierActive);
        _gameManager.ScoreMultiplier = _isMultiplierActive ? 2 : 1;
    }
}
