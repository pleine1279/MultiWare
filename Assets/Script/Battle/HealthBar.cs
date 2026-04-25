using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    public TextMeshProUGUI healthText;
    public Player player;
    void Start()
    {
        UpdateHealth(player.currentHealth, player.maxHealth);
    }
    public void UpdateHealth(float current, float max)
    {
        healthBarFill.fillAmount = current / max;

        healthText.text = $"{current} / {max}";
    }
}