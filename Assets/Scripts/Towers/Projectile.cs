using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private TowerData data;

    public float speed = 6f;

    public void Init(Enemy target, TowerData data)
    {
        this.target = target;
        this.data = data;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

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
        if (data.attackType == AttackType.SingleTarget)
        {
            target.TakeDamage(data.damage);
        }
        else if (data.attackType == AttackType.AoE)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);

            foreach (var hit in hits)
            {
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null)
                    e.TakeDamage(data.damage);
            }
        }
        else if (data.attackType == AttackType.Slow)
        {
            target.ApplySlow(0.5f, 2f);
        }

        Destroy(gameObject);
    }
}