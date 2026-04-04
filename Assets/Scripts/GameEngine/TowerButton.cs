using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TowerData towerData;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI costText;

    void Start()
    {
        costText.text = towerData.cost + " G";
        EconomyManager.Instance.OnPrepGoldChanged += RefreshButton;
        RefreshButton(EconomyManager.Instance.PrepGold);
    }

    void OnDestroy()
    {
        EconomyManager.Instance.OnPrepGoldChanged -= RefreshButton;
    }

    public void OnClick()
    {
        BuildManager.Instance.SelectTower(towerData);
    }

    private void RefreshButton(int currentGold)
    {
        button.interactable = currentGold >= towerData.cost;
    }
}