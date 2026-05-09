using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    [Header("Enemy pool")]
    public GameObject enemyPrefab;
    public int enemyPoolSize = 30;
    [Header("Projectile pool")]
    public GameObject projectilePrefab;
    public int projectilePoolSize = 50;
    [Header("Audio pool")]
    public int audioPoolSize = 20;

    private Queue<GameObject> _enemyPool = new();
    private Queue<GameObject> _projectilePool = new();
    private Queue<AudioSource> _audioPool = new();

    void Awake()
    {
        Instance = this;
        Prewarm(_enemyPool, enemyPrefab, enemyPoolSize);
        Prewarm(_projectilePool, projectilePrefab, projectilePoolSize);
        PrewarmAudio(audioPoolSize);
    }

    void PrewarmAudio(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = new GameObject("PooledAudio");
            obj.transform.SetParent(transform);
            var src = obj.AddComponent<AudioSource>();
            obj.SetActive(false);
            _audioPool.Enqueue(src);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float maxDuration = Mathf.Infinity)
    {
        if (clip == null) return;
        AudioSource src;
        if (_audioPool.Count > 0)
        {
            src = _audioPool.Dequeue();
        }
        else
        {
            var obj = new GameObject("PooledAudio");
            obj.transform.SetParent(transform);
            src = obj.AddComponent<AudioSource>();
        }
        src.transform.position = position;
        src.gameObject.SetActive(true);
        src.clip = clip;
        src.Play();
        StartCoroutine(ReturnAudio(src, Mathf.Min(maxDuration, clip.length)));
    }

    IEnumerator ReturnAudio(AudioSource src, float duration)
    {
        yield return new WaitForSeconds(duration);
        src.Stop();
        src.gameObject.SetActive(false);
        _audioPool.Enqueue(src);
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