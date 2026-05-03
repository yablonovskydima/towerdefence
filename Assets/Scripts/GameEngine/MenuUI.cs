using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    void OnEnable()
    {
        bestScoreText.text = "Best Score: " + GameManager.BestScore;
    }

    public void OnStartClicked()
    {
        GameManager.Instance.ChangeState(GameState.Preparation);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}