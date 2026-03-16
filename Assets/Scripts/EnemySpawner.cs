using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;

    public float spawnInterval = 1.5f;
    public int enemiesToSpawn = 10;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    int spawned = 0;

    void SpawnEnemy()
    {
        if (spawned >= enemiesToSpawn)
            return;

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        enemy.GetComponent<EnemyMovement>().waypoints = waypoints;

        spawned++;
    }
}