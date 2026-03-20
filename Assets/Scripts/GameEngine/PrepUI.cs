using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

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

    private TowerData selectedTowerData;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && selectedTowerData != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;

            //if (!CanPlaceTower(mousePos))
            //    return;

            Vector3 snappedPos = groundTilemap.GetCellCenterWorld(
                groundTilemap.WorldToCell(mousePos)
            );
            snappedPos.z = 0;

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
                Destroy(hit.gameObject);
        }
    }
    //TODO перевірка на ту саму тайлу

    //bool CanPlaceTower(Vector3 worldPos)
    //{
    //    Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);

    //    if (!groundTilemap.HasTile(cellPos)) return false;
    //    if (pathTilemap.HasTile(cellPos)) return false;

    //    Vector3 snappedPos = groundTilemap.GetCellCenterWorld(cellPos);
    //    snappedPos.z = 0;

    //    Collider2D existing = Physics2D.OverlapPoint(snappedPos);
    //    return existing == null || !existing.CompareTag("Tower");
    //}

    public void OnArcherSelected() => selectedTowerData = archerData;
    public void OnMageSelected() => selectedTowerData = mageData;
    public void OnFreezerSelected() => selectedTowerData = freezerData;
    public void OnCannonSelected() => selectedTowerData = cannonData;
    public void OnStartBattle() => GameManager.Instance.ChangeState(GameState.Battle);
    public void OnBackToMenu() => GameManager.Instance.ChangeState(GameState.Menu);
    public void ClearSelectedTower() => selectedTowerData = null;
}