using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public EnemyData[] enemyTypes;
    public float spawnInterval = 1.5f;

    [Header("Wave budget")]
    public int baseBudget = 100;
    public int budgetPerWave = 50;
    public int budgetIncreasePerWave = 10;

    private List<EnemyData> _waveEnemies = new List<EnemyData>();
    private int _spawned = 0;
    private int _aliveEnemies = 0;

    void Awake() { Instance = this; }

    public void StartWave()
    {
        int wave = GameManager.Instance.currentWave;
        int budget = baseBudget + budgetPerWave * (wave - 1) + budgetIncreasePerWave * (wave - 1) * (wave - 2) / 2; //quadratic increase

        _waveEnemies = BuildWave(budget);
        _spawned = 0;
        _aliveEnemies = 0;

        Debug.Log($"Wave {wave} | Budget: {budget} | Enemies: {_waveEnemies.Count}");

        StartCoroutine(SpawnWave());
    }

    List<EnemyData> BuildWave(int budget)
    {
        List<EnemyData> result = new List<EnemyData>();

        List<EnemyData> affordable = new List<EnemyData>();
        foreach (var e in enemyTypes)
            if (e.cost <= budget) affordable.Add(e);

        if (affordable.Count == 0) return result;

        int remaining = budget;

        while (remaining > 0)
        {
            List<EnemyData> canAfford = new List<EnemyData>();
            foreach (var e in affordable)
                if (e.cost <= remaining) canAfford.Add(e);

            if (canAfford.Count == 0) break;

            EnemyData chosen = canAfford[Random.Range(0, canAfford.Count)];
            result.Add(chosen);
            remaining -= chosen.cost;
        }

        for (int i = result.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }

        return result;
    }

    IEnumerator SpawnWave()
    {
        while (_spawned < _waveEnemies.Count)
        {
            if (GameManager.Instance.currentState != GameState.Battle)
            {
                yield return null;
                continue;
            }

            SpawnEnemy(_waveEnemies[_spawned]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(EnemyData data)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.data = data;
        enemyScript.OnDeath += OnEnemyDeath;
        enemy.GetComponent<EnemyMovement>().waypoints = waypoints;
        _spawned++;
        _aliveEnemies++;
    }

    void OnEnemyDeath()
    {
        _aliveEnemies--;
        if (_spawned >= _waveEnemies.Count && _aliveEnemies <= 0)
        {
            GameManager.Instance.ChangeState(GameState.RoundEnd);
        }
    }
}