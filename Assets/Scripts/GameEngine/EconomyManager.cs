using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Settings")]
    public int startingGold = 150;

    private int _prepGold;
    private int _battleGold;

    public int PrepGold => _prepGold;
    public int BattleGold => _battleGold;

    public event Action<int> OnPrepGoldChanged;
    public event Action<int> OnBattleGoldChanged;

    void Awake() { Instance = this; }

    public void InitBudget(int amount = -1)
    {
        _prepGold = amount < 0 ? startingGold : amount;
        _battleGold = 0;
        OnPrepGoldChanged?.Invoke(_prepGold);
        OnBattleGoldChanged?.Invoke(_battleGold);
    }

    public bool SpendGold(int amount)
    {
        if (_prepGold < amount) return false;
        _prepGold -= amount;
        OnPrepGoldChanged?.Invoke(_prepGold);
        return true;
    }

    public void AddBattleGold(int amount)
    {
        _battleGold += amount;
        OnBattleGoldChanged?.Invoke(_battleGold);
    }

    public void RefundGold(int amount)
    {
        _prepGold += amount;
        OnPrepGoldChanged?.Invoke(_prepGold);
    }

    public int TakePrepRemainder()
    {
        int remainder = _prepGold;
        _prepGold = 0;
        return remainder;
    }
}