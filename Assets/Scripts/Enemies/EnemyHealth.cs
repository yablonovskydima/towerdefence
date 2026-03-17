using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHP = 100;
    float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public float GetHealthPercent()
    {
        return currentHP / maxHP;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}