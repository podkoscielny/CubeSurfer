using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //private const float slowdownLength = 2f;
    private const float SLOWDOWN_FACTOR = 0.05f;

    public static TimeManager Instance { get; private set; }

    #region Singleton
    private void Awake()
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

    //void Update()
    //{
    //    Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
    //    Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    //}

    void SlowDownTheTime()
    {
        Time.timeScale = SLOWDOWN_FACTOR;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
