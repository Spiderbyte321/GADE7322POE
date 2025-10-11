using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;

    public void InitialiseHealthBar(float maxHealth)
    {
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = maxHealth;
    }

    public void SetHealth(float currentHealth)
    {
        HealthSlider.value = currentHealth;
    }
}
