using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverUI;

    [Header("Scene")]
    public string startSceneName = "Start1115";

    [SerializeField] private AudioSource BGM;

    private Player player;

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        player = FindAnyObjectByType<Player>();

        if (player != null)
        {
            player.OnDeath += GameOver;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        BGM.Pause();
        Time.timeScale = 0f;
    }
    public void ToStartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startSceneName);
    }

    void OnDestroy()
    {
        if (player != null)
        {
            player.OnDeath -= GameOver;
        }
    }
}