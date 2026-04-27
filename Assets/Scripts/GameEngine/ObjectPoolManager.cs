using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Header("Enemy pool")]
    public GameObject enemyPrefab;
    public int enemyPoolSize = 30;

    [Header("Projectile pool")]
    public GameObject projectilePrefab;
    public int projectilePoolSize = 50;

    private Queue<GameObject> _enemyPool = new();
    private Queue<GameObject> _projectilePool = new();

    void Awake()
    {
        Instance = this;
        Prewarm(_enemyPool, enemyPrefab, enemyPoolSize);
        Prewarm(_projectilePool, projectilePrefab, projectilePoolSize);
    }

    void Prewarm(Queue<GameObject> pool, GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        var obj = _enemyPool.Count > 0
            ? _enemyPool.Dequeue()
            : Instantiate(enemyPrefab, transform);

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnEnemy(GameObject obj)
    {
        obj.SetActive(false);
        _enemyPool.Enqueue(obj);
    }

    public GameObject GetProjectile(Vector3 position)
    {
        var obj = _projectilePool.Count > 0
            ? _projectilePool.Dequeue()
            : Instantiate(projectilePrefab, transform);

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnProjectile(GameObject obj)
    {
        obj.SetActive(false);
        _projectilePool.Enqueue(obj);
    }
}