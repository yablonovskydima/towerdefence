using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public static BaseHealth Instance;

    public int hp = 100;
    private int _maxHP;

    void Awake()
    {
        Instance = this;
        _maxHP = hp;
    }

    public void ResetHP()
    {
        hp = _maxHP;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Base HP: " + hp);
        if (hp <= 0)
            GameManager.Instance.ChangeState(GameState.GameOver);
    }
}