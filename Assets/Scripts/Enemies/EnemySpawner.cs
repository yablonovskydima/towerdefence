using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;

    public EnemyData[] enemyTypes;

    public float spawnInterval = 1.5f;
    public int enemiesToSpawn = 10;

    private int spawned = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (spawned >= enemiesToSpawn)
            return;

        EnemyData chosenData = enemyTypes[Random.Range(0, enemyTypes.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.data = chosenData;

        enemy.GetComponent<EnemyMovement>().waypoints = waypoints;

        spawned++;
    }
}