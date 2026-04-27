using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private TowerData data;
    public float speed = 6f;
    private SpriteRenderer sr;
    private bool hasHit = false;

    public void Init(Enemy target, TowerData data)
    {
        this.target = target;
        this.data = data;
        sr = GetComponentInChildren<SpriteRenderer>();

        if (data.projectileFlySprite != null)
            sr.sprite = data.projectileFlySprite;
    }

    void Update()
    {
        if (hasHit) return;
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            Hit();
        }
    }

    void Hit()
    {
        hasHit = true;

        if (data.attackType == AttackType.SingleTarget)
            target.TakeDamage(data.damage);
        else if (data.attackType == AttackType.AoE)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
            foreach (var hit in hits)
            {
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null) e.TakeDamage(data.damage);
            }
        }
        else if (data.attackType == AttackType.Slow)
            target.ApplySlow(0.5f, 2f);

        if (data.projectileHitSprite != null && sr != null)
        {
            sr.sprite = data.projectileHitSprite;
            StartCoroutine(ReturnAfterDelay(data.projectileHitDuration));
        }
        else
        {
            ReturnToPool();
        }
    }

    System.Collections.IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    void ReturnToPool()
    {
        hasHit = false;
        sr.sprite = null;
        ObjectPoolManager.Instance.ReturnProjectile(gameObject);
    }
}