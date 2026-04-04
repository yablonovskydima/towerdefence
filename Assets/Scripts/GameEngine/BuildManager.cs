using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private TowerData _selectedTowerData;
    public TowerData SelectedTowerData => _selectedTowerData;

    void Awake() { Instance = this; }

    public void SelectTower(TowerData data)
    {
        _selectedTowerData = data;
    }

    public void PlaceTower(Vector3 position)
    {
        if (GameManager.Instance.currentState != GameState.Preparation) return;
        if (_selectedTowerData == null) return;

        if (!EconomyManager.Instance.SpendGold(_selectedTowerData.cost))
        {
            Debug.Log("Не вистачає золота для: " + _selectedTowerData.towerName);
            return;
        }

        Instantiate(_selectedTowerData.projectilePrefab, position, Quaternion.identity);
    }

    public bool CanAfford(TowerData data)
    {
        return EconomyManager.Instance.PrepGold >= data.cost;
    }
}