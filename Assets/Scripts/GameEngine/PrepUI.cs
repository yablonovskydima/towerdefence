using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections;
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
    public TextMeshProUGUI errorText;

    [Header("Audio")]
    public AudioClip placeTowerSound;
    public AudioClip removeTowerSound;

    private TowerData selectedTowerData;
    private Coroutine errorCoroutine;

    void Start()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);

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
                ShowError("The Tower does not reach the path!");
                return;
            }
            if (!EconomyManager.Instance.SpendGold(selectedTowerData.cost))
            {
                ShowError("Not enough gold!");
                return;
            }

            GameObject tower = Instantiate(towerPrefab, snappedPos, Quaternion.identity);
            tower.GetComponent<Tower>().data = selectedTowerData;

            if (placeTowerSound != null)
                AudioSource.PlayClipAtPoint(placeTowerSound, snappedPos);
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

                if (removeTowerSound != null)
                    AudioSource.PlayClipAtPoint(removeTowerSound, hit.transform.position);

                Destroy(hit.gameObject);
            }
        }
    }

    void ShowError(string message)
    {
        if (errorText == null) return;

        if (errorCoroutine != null)
            StopCoroutine(errorCoroutine);

        errorCoroutine = StartCoroutine(ShowErrorRoutine(message));
    }

    IEnumerator ShowErrorRoutine(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorText.gameObject.SetActive(false);
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
        if (existing != null && existing.CompareTag("Tower")) return false;

        if (!IsPathInRange(snappedPos, selectedTowerData.range)) return false;

        return true;
    }

    bool IsPathInRange(Vector3 worldPos, float range)
    {
        Vector3Int center = pathTilemap.WorldToCell(worldPos);
        int radius = Mathf.CeilToInt(range / groundTilemap.cellSize.x);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int checkCell = new Vector3Int(center.x + x, center.y + y, 0);
                if (!pathTilemap.HasTile(checkCell)) continue;

                Vector3 tileWorld = pathTilemap.GetCellCenterWorld(checkCell);
                if (Vector2.Distance(worldPos, tileWorld) <= range)
                    return true;
            }
        }
        return false;
    }

    public void OnArcherSelected() => selectedTowerData = archerData;
    public void OnMageSelected() => selectedTowerData = mageData;
    public void OnFreezerSelected() => selectedTowerData = freezerData;
    public void OnCannonSelected() => selectedTowerData = cannonData;

    public void OnStartBattle()
    {
        if (GameObject.FindGameObjectsWithTag("Tower").Length == 0)
        {
            ShowError("Place at least one tower!");
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