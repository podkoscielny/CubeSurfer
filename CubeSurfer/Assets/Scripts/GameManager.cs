using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    //Theme properties
    public float CurrentSpeed { get; set; }
    public Color FogColor { get; private set; }
    public Color LightingGradient { get; private set; }
    public Material SkyboxMaterial { get; private set; }
    public Material PlayerMaterial { get; private set; }
    public Material GroundMaterial { get; private set; }
    public Color SkyboxTopColor { get; private set; }
    public Color SkyboxBottomColor { get; private set; }

    //Game state
    public bool IsGameOver { get; private set; } = false;
    public bool HasGameStarted { get; private set; } = false;

    //Score and level
    public int ScoreMultiplier { get; set; } = 1;
    public int Level { get; private set; } = 1;
    private float _score = 0;
    private const float SCORE_DIVIDER = 2;

    //Highscores
    public bool IsHighscoreSet { get; private set; } = false;
    public List<Highscore> Highscores { get; private set; }
    private const int HIGHSCORE_LIMIT = 5;

    //HUD Texts
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _levelText;

    protected override void Awake()
    {
        isDestroyableOnLoad = false;
        base.Awake();
    }

    #region Event Subscribers
    private void OnEnable()
    {
        MoveObstacleToPool.OnLevelup += LevelUp;
        PlayerController.OnGameOver += GameOver;
        StartGameCamera.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        MoveObstacleToPool.OnLevelup -= LevelUp;
        PlayerController.OnGameOver -= GameOver;
        StartGameCamera.OnGameStart -= StartGame;
    }
    #endregion

    private void Start() => SaveSystem.Load();

    private void Update()
    {
        if (HasGameStarted && !IsGameOver && _scoreText != null) UpdateScore();
    }

    public void SetTheme(Material skyboxMaterial, Color fogColor, Color lightingGradient, Material groundMaterial, Material playerMaterial) // set colors in options menu
    {
        SkyboxMaterial = skyboxMaterial;
        FogColor = fogColor;
        LightingGradient = lightingGradient;
        GroundMaterial = groundMaterial;
        PlayerMaterial = playerMaterial;

        SkyboxTopColor = skyboxMaterial.GetColor("_SkyGradientTop");
        SkyboxBottomColor = skyboxMaterial.GetColor("_SkyGradientBottom");
    }

    public void ResetGame()
    {
        IsGameOver = false;
        HasGameStarted = false;
        IsHighscoreSet = false;
        Level = 1;
        _score = 0;
        //Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f;

        SkyboxMaterial.SetColor("_SkyGradientTop", SkyboxTopColor);
        SkyboxMaterial.SetColor("_SkyGradientBottom", SkyboxBottomColor);
    }

    public void GetHUDTexts(TextMeshProUGUI scoreHudText, TextMeshProUGUI levelHudText)
    {
        _scoreText = scoreHudText;
        _levelText = levelHudText;
    }

    public void LoadHighscores(List<Highscore> highscores) => Highscores = highscores;

    private void StartGame() => HasGameStarted = true;

    private void GameOver()
    {
        IsGameOver = true;

        int currentScorePlacement = GetCurrentScorePlacement();

        if (currentScorePlacement <= HIGHSCORE_LIMIT - 1) SetHighscore(currentScorePlacement);
    }

    private void SetHighscore(int scorePlacement)
    {
        Highscore newHighscore = new Highscore((int)_score, Level);
        Highscores.Insert(scorePlacement, newHighscore);
        IsHighscoreSet = true;

        if (Highscores.Count > HIGHSCORE_LIMIT) Highscores.RemoveAt(Highscores.Count - 1);

        SaveSystem.Save(Highscores);
    }

    private int GetCurrentScorePlacement()
    {
        if (Highscores.Count > 0)
        {
            for (int i = 0; i < Highscores.Count; i++)
            {
                if ((int)_score > Highscores[i].score) return i;
            }
        }

        return Highscores.Count;
    }

    private void UpdateScore()
    {
        _score += CurrentSpeed / SCORE_DIVIDER * Time.deltaTime * ScoreMultiplier;
        int roundScore = (int)_score;
        _scoreText.text = roundScore.ToString();
    }

    private void LevelUp()
    {
        Level++;
        _levelText.text = $"Level: {Level}";
    }
}
