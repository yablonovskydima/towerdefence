using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}