using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentWave = 1;
    public GameState currentState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.Menu);
    }

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
                int earned = EconomyManager.Instance.BattleGold;
                EconomyManager.Instance.InitBudget(earned > 0 ? earned : -1);
                UIManager.Instance.ShowPreparation();
                break;

            case GameState.Battle:
                UIManager.Instance.ShowBattle();
                WaveManager.Instance.StartWave();
                break;

            case GameState.RoundEnd:
                UIManager.Instance.ShowRoundEnd(EconomyManager.Instance.BattleGold);
                break;

            case GameState.GameOver:
                UIManager.Instance.ShowGameOver();
                break;
        }
    }
}