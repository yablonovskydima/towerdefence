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
}

public enum AttackType
{
    SingleTarget,
    AoE,
    Slow
}