using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public HealthBar healthBar;

    void Start()
    {
        player.OnHealthChanged += healthBar.UpdateHealth;
    }
}