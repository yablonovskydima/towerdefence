using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Slider slider;

    void Update()
    {
        if (enemy != null)
            slider.value = enemy.GetHealthPercent();
    }
}