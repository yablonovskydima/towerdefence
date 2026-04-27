using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    [HideInInspector] public float currentHP;
    private float baseSpeed;
    private float currentSpeed;
    private float _slowTimer = 0f;
    public System.Action OnDeath;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        OnDeath = null;
        currentHP = data.maxHP;
        baseSpeed = data.speed;
        currentSpeed = baseSpeed;
        _slowTimer = 0f;

        if (data.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;
        if (data.sprite != null)
            GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    void Update()
    {
        if (_slowTimer > 0f)
        {
            _slowTimer -= Time.deltaTime;
            if (_slowTimer <= 0f)
                currentSpeed = baseSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
    }

    public void ApplySlow(float percent, float duration)
    {
        if (data.ignoreFreezer) return;
        currentSpeed = baseSpeed * percent;
        _slowTimer = duration;
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
        var cb = OnDeath;
        OnDeath = null;
        cb?.Invoke();
        ObjectPoolManager.Instance.ReturnEnemy(gameObject);
    }

    public void ReachBase()
    {
        var cb = OnDeath;
        OnDeath = null;
        cb?.Invoke();
        ObjectPoolManager.Instance.ReturnEnemy(gameObject);
    }
}