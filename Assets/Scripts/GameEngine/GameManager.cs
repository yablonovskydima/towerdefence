using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentWave = 0;
    public GameState currentState;
    private int _earnedGold = 0;
    private int _prepRemainder = 0;  // ← ДОДАТИ ЦЕ ПОЛЕ

    [Header("Settings")]
    public int baseHP = 100;

    void Awake() { Instance = this; }
    void Start() { ChangeState(GameState.Menu); }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.Menu:
                UIManager.Instance.ShowMenu();
                break;

            case GameState.Preparation:
                // Загальний бюджет = залишок підготовки + зароблене в бою
                int totalGold = _prepRemainder + _earnedGold;
                EconomyManager.Instance.InitBudget(totalGold > 0 ? totalGold : -1);
                _earnedGold = 0;
                _prepRemainder = 0;
                UIManager.Instance.ShowPreparation();
                break;

            case GameState.Battle:
                // ← ЗБЕРІГАЄМО ЗАЛИШОК ПЕРЕД ПОЧАТКОМ БОЮ
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
                UIManager.Instance.ShowGameOver();
                break;
        }
    }

    public void RestartGame()
    {
        currentWave = 0;
        _earnedGold = 0;
        _prepRemainder = 0;  // ← СКИДАТИ І ЦЕ
        BaseHealth.Instance.ResetHP();
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
            Destroy(tower);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        EconomyManager.Instance.InitBudget(-1);
        ChangeState(GameState.Preparation);
    }
}