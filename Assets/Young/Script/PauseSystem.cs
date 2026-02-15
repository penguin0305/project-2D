using UnityEngine;
using UnityEditor;
using SupanthaPaul;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    [SerializeField] private AudioSource BGM;
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (InputSystem.Pause())
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        BGM.UnPause();

        Debug.Log("게임 재개");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        BGM.Pause();

        Debug.Log("게임 일시 정지");
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Game End");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}