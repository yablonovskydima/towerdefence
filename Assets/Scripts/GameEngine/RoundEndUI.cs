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
        GameManager.Instance.TrySaveBestScore();

        GameManager.Instance.currentWave = 0;
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
            Destroy(tower);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        EconomyManager.Instance.InitBudget(-1);
        BaseHealth.Instance.ResetHP();

        GameManager.Instance.ChangeState(GameState.Menu);
    }
}