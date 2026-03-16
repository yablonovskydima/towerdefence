using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int hp = 20;

    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log("Base HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}