using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public EnemyData[] enemyTypes;
    public float spawnInterval = 1.5f;
    public int enemiesToSpawn = 10;

    private int spawned = 0;
    private int aliveEnemies = 0;

    void Awake()
    {
        Instance = this;
    }

    public void StartWave()
    {
        GameManager.Instance.currentWave++;
        spawned = 0;
        aliveEnemies = 0;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (spawned < enemiesToSpawn)
        {
            if (GameManager.Instance.currentState != GameState.Battle)
            {
                yield return null;
                continue;
            }

            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        EnemyData chosenData = enemyTypes[Random.Range(0, enemyTypes.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.data = chosenData;
        enemyScript.OnDeath += OnEnemyDeath;

        enemy.GetComponent<EnemyMovement>().waypoints = waypoints;

        spawned++;
        aliveEnemies++;
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        if (spawned >= enemiesToSpawn && aliveEnemies <= 0)
        {
            GameManager.Instance.ChangeState(GameState.RoundEnd);
        }
    }
}