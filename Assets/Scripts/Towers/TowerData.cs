using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int cost;

    public float range;
    public float fireRate;
    public float damage;

    public AttackType attackType;

    public GameObject projectilePrefab;

    [Header("Visuals")]
    public Sprite idleSprite;
    public Sprite shootSprite;
    public float shootSpriteDuration = 0.1f;

    [Header("Projectile Visuals")]
    public Sprite projectileFlySprite;
    public Sprite projectileHitSprite;
    public float projectileHitDuration = 0.1f;
}

public enum AttackType
{
    SingleTarget,
    AoE,
    Slow
}