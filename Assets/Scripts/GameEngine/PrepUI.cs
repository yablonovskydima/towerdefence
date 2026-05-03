using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using TMPro;

public class PrepUI : MonoBehaviour
{
    public GameObject towerPrefab;
    public TowerData archerData;
    public TowerData mageData;
    public TowerData freezerData;
    public TowerData cannonData;

    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap pathTilemap;

    [Header("UI")]
    public TextMeshProUGUI goldText;

    private TowerData selectedTowerData;

    void Start()
    {
        EconomyManager.Instance.OnPrepGoldChanged += UpdateGoldText;
    }

    void OnEnable()
    {
        if (EconomyManager.Instance == null) return;
        EconomyManager.Instance.OnPrepGoldChanged += UpdateGoldText;
        UpdateGoldText(EconomyManager.Instance.PrepGold);
    }

    void OnDisable()
    {
        if (EconomyManager.Instance == null) return;
        EconomyManager.Instance.OnPrepGoldChanged -= UpdateGoldText;
    }

    void UpdateGoldText(int amount)
    {
        if (goldText == null) return;
        goldText.text = amount + " G";
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && selectedTowerData != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;

            Vector3 snappedPos = groundTilemap.GetCellCenterWorld(groundTilemap.WorldToCell(mousePos));
            snappedPos.z = 0;

            if (!CanPlaceTower(mousePos))
            {
                Debug.Log("Cannot place tower here!");
                return;
            }
            if (!EconomyManager.Instance.SpendGold(selectedTowerData.cost))
            {
                Debug.Log("Not enough gold!");
                return;
            }

            GameObject tower = Instantiate(towerPrefab, snappedPos, Quaternion.identity);
            tower.GetComponent<Tower>().data = selectedTowerData;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.CompareTag("Tower"))
            {
                Tower tower = hit.GetComponent<Tower>();
                if (tower != null)
                    EconomyManager.Instance.RefundGold(tower.data.cost / 2);
                Destroy(hit.gameObject);
            }
        }
    }

    bool CanPlaceTower(Vector3 worldPos)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        if (!groundTilemap.HasTile(cellPos)) return false;
        if (pathTilemap.HasTile(cellPos)) return false;

        Vector3 snappedPos = groundTilemap.GetCellCenterWorld(cellPos);
        snappedPos.z = 0;

        Vector2 cellSize = groundTilemap.cellSize * 0.9f;
        Collider2D existing = Physics2D.OverlapBox(snappedPos, cellSize, 0f);

        if (existing != null)
            Debug.Log($"Blocked by: {existing.name} tag:{existing.tag} at {existing.transform.position}");

        return existing == null || !existing.CompareTag("Tower");
    }

    public void OnArcherSelected() => selectedTowerData = archerData;
    public void OnMageSelected() => selectedTowerData = mageData;
    public void OnFreezerSelected() => selectedTowerData = freezerData;
    public void OnCannonSelected() => selectedTowerData = cannonData;
    public void OnStartBattle()
    {
        if (GameObject.FindGameObjectsWithTag("Tower").Length == 0)
        {
            Debug.Log("At least one tower needs to be placed!");
            return;
        }
        GameManager.Instance.ChangeState(GameState.Battle);
    }
    public void OnBackToMenu()
    {
        GameManager.Instance.TrySaveBestScore();

        GameManager.Instance.currentWave = 0;

        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
            Destroy(tower);

        EconomyManager.Instance.InitBudget(-1);
        GameManager.Instance.ChangeState(GameState.Menu);
    }

    public void ClearSelectedTower() => selectedTowerData = null;
}