using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;
    public HealthBar healthBar;

    void Start()
    {
        player.OnHealthChanged += healthBar.UpdateHealth;
    }
}
