using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;

    private int waypointIndex = 0;
    private Enemy enemy;
    private bool reachedBase = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (reachedBase) return;

        if (waypointIndex >= waypoints.Length)
        {
            ReachBase();
            return;
        }

        Transform target = waypoints[waypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            enemy.getCurrentSpeed() * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 0.05f)
        {
            waypointIndex++;
        }
    }

    void ReachBase()
    {
        reachedBase = true;
        BaseHealth.Instance.TakeDamage(enemy.data.damage);
        enemy?.ReachBase();
    }
}