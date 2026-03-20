using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;

    private int waypointIndex = 0;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
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
        BaseHealth baseHealth = FindObjectOfType<BaseHealth>();
        Enemy enemy = GetComponent<Enemy>();
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(enemy.data.damage);
        }
        
        enemy?.ReachBase();
    }
}