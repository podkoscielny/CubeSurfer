using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Texts")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("Pause Game")]
    [SerializeField] GameObject pausePopUp;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject playButton;
    [SerializeField] AudioSource backgroundMusic;

    [Header("Game Over Panel")]
    [SerializeField] GameObject newHighscoreText;
    [SerializeField] GameObject gameOverPanel;


    private GameManager _gameManager;

    private bool _isGamePaused = false;
    private const float SHOW_POPUP_DELAY = 3.5f;
    private const float TURNED_DOWN_VOLUME = 0.03f;
    private const float REGULAR_VOLUME = 0.1f;

    #region Event Subscribers

    void OnEnable()
    {
        PlayerController.OnGameOver += ShowGameOverPanel;
    }

    void OnDisable()
    {
        PlayerController.OnGameOver -= ShowGameOverPanel;
    }

    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.GetHUDTexts(scoreText, levelText);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if (!_gameManager.IsGameOver)
        {
            _isGamePaused = false;
            Time.timeScale = 1f;
            backgroundMusic.volume = REGULAR_VOLUME;

            pausePopUp.SetActive(false);
            playButton.SetActive(false);
            pauseButton.SetActive(true);
        }
    }

    public void PauseGame()
    {
        if (!_gameManager.IsGameOver)
        {
            _isGamePaused = true;
            Time.timeScale = 0f;
            backgroundMusic.volume = TURNED_DOWN_VOLUME;

            pausePopUp.SetActive(true);
            playButton.SetActive(true);
            pauseButton.SetActive(false);
        }
    }


    private void ShowGameOverPanel() => StartCoroutine(DelayShowingPanel());

    IEnumerator DelayShowingPanel()
    {
        yield return new WaitForSecondsRealtime(SHOW_POPUP_DELAY);

        gameOverPanel.SetActive(true);
        if (_gameManager.IsHighscoreSet) newHighscoreText.SetActive(true);
    }
}
