using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject selectedTower;

    void Awake()
    {
        Instance = this;
    }

    public void SelectTower(GameObject tower)
    {
        selectedTower = tower;
    }

    public void PlaceTower(Vector3 position)
    {
        if (GameManager.Instance.currentState != GameState.Preparation)
            return;

        Instantiate(selectedTower, position, Quaternion.identity);
    }
}