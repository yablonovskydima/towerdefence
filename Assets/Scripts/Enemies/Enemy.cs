using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    [HideInInspector]
    public float currentHP;

    public float speed;

    void Start()
    {
        currentHP = data.maxHP;
        speed = data.speed;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}