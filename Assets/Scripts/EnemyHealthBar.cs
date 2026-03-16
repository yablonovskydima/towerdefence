using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemy;
    public Slider slider;

    void Update()
    {
        slider.value = enemy.GetHealthPercent();
    }
}