using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverUI;

    [Header("Scene")]
    public string startSceneName = "Start1115";

    private PlayerStats playerStats;

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.OnDeath += StageEnd;
        }
    }

    private void StageEnd()
    {
        Debug.Log("Game Over");

        if (gameOverUI != null)
            gameOverUI.SetActive(true);


            Time.timeScale = 0f;
    }
    public void ToStartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startSceneName);
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnDeath -= StageEnd;
        }
    }
}