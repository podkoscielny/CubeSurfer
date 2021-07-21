using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Animator transition;
    [SerializeField] Image progressFill;

    private const float TRANSITION_TIME = 1f;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.ResetGame();
    }

    public void StartGame() => StartCoroutine(LoadScene("GamePlay"));

    public void QuitGame() => Application.Quit();

    public void RestartGame() => StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));

    public void GoToMenu() => StartCoroutine(LoadScene("MainMenu"));

    IEnumerator LoadScene(string sceneName)
    {
        transition.SetTrigger("Hide");

        yield return new WaitForSecondsRealtime(TRANSITION_TIME);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        progressBar.gameObject.SetActive(true);
        progressFill.color = _gameManager.Theme.backgroundColor;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = $"{(int)(progress * 100f)}%";

            yield return null;
        }
    }
}
