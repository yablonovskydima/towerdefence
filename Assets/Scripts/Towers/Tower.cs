using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData data;
    public Transform firePoint;
    private float cooldown;
    private SpriteRenderer sr;
    private float shootSpriteTimer;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if (data.idleSprite != null)
            sr.sprite = data.idleSprite;
    }

    void Update()
    {
        if (shootSpriteTimer > 0f)
        {
            shootSpriteTimer -= Time.deltaTime;
            if (shootSpriteTimer <= 0f && data.idleSprite != null)
                sr.sprite = data.idleSprite;
        }

        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            Enemy target = GetTarget();
            if (target != null)
            {
                Shoot(target);
                cooldown = 1f / data.fireRate;
            }
        }
    }

    Enemy GetTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data.range);
        Enemy closest = null;
        float minDist = Mathf.Infinity;
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }

    void Shoot(Enemy target)
    {
        GameObject proj = ObjectPoolManager.Instance.GetProjectile(firePoint.position);
        proj.GetComponent<Projectile>().Init(target, data);

        if (data.shootSprite != null)
        {
            sr.sprite = data.shootSprite;
            shootSpriteTimer = data.shootSpriteDuration;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}