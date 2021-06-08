using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private const float SLOWDOWN_FACTOR = 0.05f;

    #region Event Subscribers

    private void OnEnable()
    {
        PlayerController.OnGameOver += SlowDownTheTime;
    }

    private void OnDisable()
    {
        PlayerController.OnGameOver -= SlowDownTheTime;
    }

    #endregion

    private void Start() => ResetTime();

    void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void SlowDownTheTime()
    {
        Time.timeScale = SLOWDOWN_FACTOR;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
