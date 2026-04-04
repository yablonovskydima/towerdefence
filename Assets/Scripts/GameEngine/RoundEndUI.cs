using UnityEngine;
using TMPro;

public class RoundEndUI : MonoBehaviour
{
    public TextMeshProUGUI goldEarnedText;

    public void Setup(int goldEarned)
    {
        goldEarnedText.text = "Gold earned: " + goldEarned;
    }

    public void OnContinueClicked()
    {
        GameManager.Instance.ChangeState(GameState.Preparation);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}