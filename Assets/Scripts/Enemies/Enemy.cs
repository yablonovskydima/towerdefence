using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    [HideInInspector] public float currentHP;

    private float baseSpeed;
    private float currentSpeed;
    public System.Action OnDeath;
    private Coroutine slowCoroutine;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = data.maxHP;
        baseSpeed = data.speed;
        currentSpeed = baseSpeed;
    }

    public void Initialize()
    {
        currentHP = data.maxHP;
        baseSpeed = data.speed;
        currentSpeed = baseSpeed;

        if (data.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;

        if (data.sprite != null)
            GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float percent, float duration)
    {

        if (data.ignoreFreezer)
            return;

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowRoutine(percent, duration));
    }

    IEnumerator SlowRoutine(float percent, float duration)
    {
        currentSpeed = baseSpeed * percent;

        yield return new WaitForSeconds(duration);

        currentSpeed = baseSpeed;
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetHealthPercent()
    {
        return currentHP / data.maxHP;
    }

    void Die()
    {
        EconomyManager.Instance.AddBattleGold(data.goldReward);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void ReachBase()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}