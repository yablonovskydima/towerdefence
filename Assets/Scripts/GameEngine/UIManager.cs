using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject menuUI;
    public GameObject prepUI;
    public GameObject battleUI;
    public GameObject roundEndUI;
    public GameObject gameOverUI;

    void Awake()
    {
        Instance = this;
    }

    void HideAll()
    {
        menuUI.SetActive(false);
        prepUI.SetActive(false);
        battleUI.SetActive(false);
        roundEndUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void ShowMenu()
    {
        HideAll();
        menuUI.SetActive(true);
    }

    public void ShowPreparation()
    {
        HideAll();
        prepUI.SetActive(true);

        PrepUI prep = prepUI.GetComponent<PrepUI>();
        prep.ClearSelectedTower();
    }

    public void ShowBattle()
    {
        HideAll();
        battleUI.SetActive(true);

        BattleUI battle = battleUI.GetComponent<BattleUI>();
        battle.UpdateWave(GameManager.Instance.currentWave);
    }

    public void ShowRoundEnd(int goldEarned = 0)
    {
        HideAll();
        roundEndUI.SetActive(true);

        RoundEndUI roundEnd = roundEndUI.GetComponent<RoundEndUI>();
        roundEnd.Setup(goldEarned);
    }

    public void ShowGameOver()
    {
        HideAll();
        gameOverUI.SetActive(true);
    }
}