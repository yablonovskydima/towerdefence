using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentWave = 0;
    public GameState currentState;
    private int _earnedGold = 0;  // ← додай це поле
    [Header("Settings")]
    public int baseHP = 100;

    void Awake() { Instance = this; }
    void Start() { ChangeState(GameState.Menu); }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("STATE: " + newState);
        switch (newState)
        {
            case GameState.Menu:
                UIManager.Instance.ShowMenu();
                break;

            case GameState.Preparation:
                Debug.Log($"Preparation: earnedGold = {_earnedGold}, BattleGold = {EconomyManager.Instance.BattleGold}");
                EconomyManager.Instance.InitBudget(_earnedGold > 0 ? _earnedGold : -1);
                _earnedGold = 0;
                UIManager.Instance.ShowPreparation();
                break;

            case GameState.Battle:
                currentWave++;
                UIManager.Instance.ShowBattle();
                WaveManager.Instance.StartWave();
                break;

            case GameState.RoundEnd:
                WaveManager.Instance.CleanupEnemies();  // ← спочатку прибираємо ворогів
                _earnedGold = EconomyManager.Instance.BattleGold;  // ← тепер берємо фінальне золото
                Debug.Log($"RoundEnd: saving earnedGold = {_earnedGold}");
                UIManager.Instance.ShowRoundEnd(_earnedGold);
                break;

            case GameState.GameOver:
                UIManager.Instance.ShowGameOver();
                break;
        }
    }

    public void RestartGame()
    {
        currentWave = 0;
        _earnedGold = 0;
        BaseHealth.Instance.ResetHP();
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
            Destroy(tower);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        EconomyManager.Instance.InitBudget(-1);
        ChangeState(GameState.Preparation);
    }
}