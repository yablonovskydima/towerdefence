using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;

    int waypointIndex = 0;

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
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            waypointIndex++;
        }
    }

    void ReachBase()
    {
        BaseHealth baseHealth = FindObjectOfType<BaseHealth>();

        if (baseHealth != null)
        {
            baseHealth.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}