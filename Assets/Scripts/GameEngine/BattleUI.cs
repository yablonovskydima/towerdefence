using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI goldText;

    void OnEnable()
    {
        EconomyManager.Instance.OnBattleGoldChanged += UpdateGold;
        UpdateGold(EconomyManager.Instance.BattleGold);
    }

    void OnDisable()
    {
        EconomyManager.Instance.OnBattleGoldChanged -= UpdateGold;
    }

    public void UpdateWave(int wave)
    {
        waveText.text = "Wave: " + wave;
    }

    void UpdateGold(int amount)
    {
        goldText.text = amount + " G";
    }
}