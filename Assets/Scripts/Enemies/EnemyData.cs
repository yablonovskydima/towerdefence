using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "TD/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public float maxHP;
    public float speed;

    public int cost;

    public bool ignoreFreezer;
    public int damage = 1;
    public int goldReward = 20;

    [Header("Audio")]
    public AudioClip deathSound;

    [Header("Visuals")]
    public RuntimeAnimatorController animatorController;
    public Sprite sprite;
}