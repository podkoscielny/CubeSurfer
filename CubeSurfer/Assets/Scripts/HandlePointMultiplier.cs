﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandlePointMultiplier : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip pickedMultiplierSound;
    private AudioSource _playerAudio;
    private const float SOUND_VOLUME = 1f;

    [Header("Unity Events")]
    [SerializeField] UnityEvent OnEnableMultiplier;
    [SerializeField] UnityEvent OnDisableMultiplier;

    private const float multiplierDuration = 10f;

    void Start() => _playerAudio = GetComponent<AudioSource>();

    private void DisableMultiplier() => OnDisableMultiplier.Invoke();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PointMultiplier"))
        {
            OnEnableMultiplier.Invoke();
            _playerAudio.PlayOneShot(pickedMultiplierSound, SOUND_VOLUME);
            other.gameObject.SetActive(false);
            Invoke(nameof(DisableMultiplier), multiplierDuration);
        }
    }
}
