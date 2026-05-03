using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentWave = 0;
    public GameState currentState;
    private int _earnedGold = 0;
    private int _prepRemainder = 0;

    [Header("Settings")]
    public int baseHP = 100;

    public static int BestScore => PlayerPrefs.GetInt("BestScore", 0);

    void Awake() { Instance = this; }
    void Start() { ChangeState(GameState.Menu); }

    public void TrySaveBestScore()
    {
        if (currentWave > BestScore)
            PlayerPrefs.SetInt("BestScore", currentWave);
        PlayerPrefs.Save();
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.Menu:
                UIManager.Instance.ShowMenu();
                break;
            case GameState.Preparation:
                int totalGold = _prepRemainder + _earnedGold;
                EconomyManager.Instance.InitBudget(totalGold > 0 ? totalGold : -1);
                _earnedGold = 0;
                _prepRemainder = 0;
                UIManager.Instance.ShowPreparation();
                break;
            case GameState.Battle:
                _prepRemainder = EconomyManager.Instance.TakePrepRemainder();
                currentWave++;
                UIManager.Instance.ShowBattle();
                WaveManager.Instance.StartWave();
                break;
            case GameState.RoundEnd:
                WaveManager.Instance.CleanupEnemies();
                _earnedGold = EconomyManager.Instance.BattleGold;
                UIManager.Instance.ShowRoundEnd(_earnedGold);
                break;
            case GameState.GameOver:
                TrySaveBestScore();
                UIManager.Instance.ShowGameOver();
                break;
        }
    }

    public void RestartGame()
    {
        currentWave = 0;
        _earnedGold = 0;
        _prepRemainder = 0;
        BaseHealth.Instance.ResetHP();
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
            Destroy(tower);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        EconomyManager.Instance.InitBudget(-1);
        ChangeState(GameState.Preparation);
    }
}