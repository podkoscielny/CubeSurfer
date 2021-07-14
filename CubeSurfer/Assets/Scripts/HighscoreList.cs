using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI noHighscoresText;
    [SerializeField] List<TextMeshProUGUI> highscoresList;

    void Start() => DisplayHighscoreList();

    void DisplayHighscoreList()
    {
        List<Highscore> highscores = GameManager.Instance.Highscores;

        if (highscores.Count == 0)
        {
            noHighscoresText.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < highscores.Count; i++)
            {
                highscoresList[i].gameObject.SetActive(true);
                highscoresList[i].text = $"#{i + 1} Score: {highscores[i].score} | Level: {highscores[i].level}";
            }
        }
    }
}
