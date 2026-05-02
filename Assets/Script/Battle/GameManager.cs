using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene MonsterSpawn"); // 여기에 전투씬 이름 정확하게
    }

    public void OnVictory()
    {
        // 추후 승리 시 할 행동 추가
        Debug.Log("전투 승리! 상점으로 이동");
        SceneManager.LoadScene("ShopScene"); // ← ShopScene 이름 일단 예시로 적어놈
    }

    public void OnDefeat()
    {
        Debug.Log("전투 패배!");
        // 나중에 GameOver 씬 등 추가해야함
    }

    public void OnShopDone()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
