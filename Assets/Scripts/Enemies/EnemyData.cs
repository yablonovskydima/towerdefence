using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "TD/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public float maxHP;
    public float speed;

    public int cost;

    public bool ignoreFreezer;
}